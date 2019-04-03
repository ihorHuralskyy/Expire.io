using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.ViewModels;

namespace Expire.io.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ExpireContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ExpireContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                        return RedirectToAction("Index", controllerName: "Home");
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
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
    }
}