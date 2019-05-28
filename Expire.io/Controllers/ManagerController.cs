using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Expire.io.DTOs;
using Expire.io.Helpers;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Expire.io.Controllers
{
    public class ManagerController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ExpireContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public ManagerController(UserManager<User> userManager, ExpireContext context, SignInManager<User> sm, RoleManager<Role> rm)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = sm;
            _roleManager = rm;
        }

        public IActionResult AllUsersByManager()
        {
            int managerId = _context.Users.FirstOrDefault(item => item.UserName == User.Identity.Name).Id;
            var list = _context.Users.Where(item =>
                _context.ManagerUsers.Where(i => i.ManagerId == managerId).Any(u => u.UserId == item.Id)).Select(p=> new UserForManagerDTO{Id = p.Id, UserName = p.UserName}).ToList();
            return View(list);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Notify(string email)
        {
            EmailSender sender = new EmailSender();
            var user = _context.Users.FirstOrDefault(item => item.UserName == email);
            sender.SendEmail(email, user.FirstName, user.LastName);
            return Ok();
        }

    }
}