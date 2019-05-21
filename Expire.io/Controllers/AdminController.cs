using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.DTOs;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Diagnostics;
using Expire.io.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace Expire.io.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ExpireContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        public AdminController(UserManager<User> userManager, ExpireContext context, SignInManager<User> sg, RoleManager<Role> rm)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = sg;
            _roleManager = rm;
        }
        public async Task<IActionResult> AllUsers()
        {
            var allUsers = _userManager.Users.Select(user =>
                new UserDTORolesList
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    UserName = user.UserName
                }).ToList();

            foreach (var item in allUsers)
            {
                item.Roles = await _userManager.GetRolesAsync(_userManager.Users.Where(u => u.UserName == item.UserName).First());
            }
            return View(allUsers.ToList());
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AdminAddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User()
                    {
                        FirstName = model.FName,
                        LastName = model.LName,
                        Email = model.Email,
                        UserName = model.Email
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        Response.StatusCode = 200;
                        return Json(new { Success = true });
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, item.Description);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return BadRequest("Your data is not valid");
        }


        public async Task<IActionResult> DeleteUser(string username)
        {

            var user = _userManager.Users.Where(u => u.UserName == username).First();
            int Id = user.Id;
            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded == true)
            {
                return Ok(Json(new { resp = "User was deleted", id = Id }));
            }
            else
            {
                return BadRequest(Json(new { resp = "Unexpected server error" }));
            }
        }

        public IActionResult MakeUserAnAdmin(string username)
        {
            var user = _userManager.Users.Where(u => u.UserName == username).First();
            int Id = user.Id;
            var result = _userManager.AddToRoleAsync(user, "Admin").Result;
            if (result.Succeeded == true)
            {
                return Ok(Json(new { resp = user.UserName.ToString() + " was added to role Admin ", id = Id }));
            }
            else
            {
                return BadRequest(Json(new { resp = "Unexpected server error" }));
            }
        }

        public IActionResult MakeUserAnManager(string username)
        {
            var user = _userManager.Users.Where(u => u.UserName == username).First();
            int Id = user.Id;
            var result = _userManager.AddToRoleAsync(user, "Manager").Result;
            if (result.Succeeded == true)
            {
                return Ok(Json(new { resp = user.UserName.ToString() + " was added to role Manager ", id = Id }));
            }
            else
            {
                return BadRequest(Json(new { resp = "Unexpected server error" }));
            }
        }

        public IActionResult GetManagerList()
        {

            int roleId = _roleManager.Roles.FirstOrDefault(item => item.Name == "manager").Id;

            var list = _context.UserRoles.Where(item => item.RoleId == roleId).Select(u => _userManager.Users.FirstOrDefault(uu => uu.Id == u.UserId).UserName).ToList();

            return PartialView(list);
        }

        public IActionResult GiveToManager(string managerName, int userId)
        {
            int managerId = _userManager.Users.FirstOrDefault(item => item.UserName == managerName).Id;

            var res = _context.ManagerUsers.FirstOrDefault(item =>
                item.ManagerId == managerId && item.UserId == userId);
            if (res != null)
            {
                return Json(new { resp = "User is already added" });
            }
            else
            {
                ManagerUser managerUser = new ManagerUser
                {
                    UserId = userId,
                    ManagerId = managerId
                };

                _context.ManagerUsers.Add(managerUser);
                _context.SaveChanges();
                HttpContext.Response.StatusCode = 200;
                return Json(new { resp = "User was added to manager" });
            }
        }
    }
}