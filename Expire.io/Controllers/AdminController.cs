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

namespace Expire.io.Controllers
{
    
    public class AdminController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ExpireContext _context;
        private readonly SignInManager<User> _signInManager;
        public AdminController(UserManager<User> userManager, ExpireContext context, SignInManager<User> sg)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = sg;
        }
        public async Task<IActionResult> AllUsers()
        {
            var allUsers = _userManager.Users.Select(user =>
           new UserDTO{
               Id = user.Id,
               FirstName = user.FirstName,
               LastName = user.LastName,
               Email = user.Email,
               Phone = user.PhoneNumber,
               UserName = user.UserName,
               Roles = user.UserRoles.Select(role=>new RoleDTO
               {
                   Id = role.RoleId,
                   Name = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
               }).ToList()
            });

            return View(allUsers);
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
                        await _signInManager.SignInAsync(user, isPersistent: true);
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
    }
}