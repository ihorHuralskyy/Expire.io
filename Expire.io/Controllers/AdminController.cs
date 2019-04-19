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
        public AdminController(UserManager<User> userManager, ExpireContext context)
        {
            _userManager = userManager;
            _context = context;
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
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
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
                    UserImage image = null;
                    if (model.Photo != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            model.Photo.CopyTo(ms);
                            byte[] img = ms.ToArray();
                            image = new UserImage { Image = img };
                        }
                    }
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (image != null)
                        {
                            image.User = user;
                            _context.UserImages.AddAsync(image).Wait();
                            await _context.SaveChangesAsync();
                        }
                        await _userManager.AddToRoleAsync(user, "User");
                        await _signInManager.SignInAsync(user, isPersistent: true);
                        return Ok();
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