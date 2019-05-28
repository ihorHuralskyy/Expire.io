using Expire.io.DTOs;
using Expire.io.Helpers;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Expire.io.Services
{
    public class ManagerService : IManagerService
    {
        private readonly UserManager<User> _userManager;
        private readonly ExpireContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _httpContext;

        public ManagerService(UserManager<User> userManager, 
            ExpireContext context, SignInManager<User> signInManager, 
            RoleManager<Role> roleManager,
            IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContext = accessor;
        }

        public List<UserForManagerDTO> AllUsersByManager()
        {
            int managerId = _context.Users
                .FirstOrDefault(item => item.UserName == _httpContext.HttpContext.User.Identity.Name).Id;
            var list = _context.Users.Where(item =>
                _context.ManagerUsers
                .Where(i => i.ManagerId == managerId).Any(u => u.UserId == item.Id))
                .Select(p => new UserForManagerDTO { Id = p.Id, UserName = p.UserName }).ToList();
            return list;
        }

        public void Notify(string email)
        {
            EmailSender sender = new EmailSender();
            var user = _context.Users.FirstOrDefault(item => item.UserName == email);
            sender.SendEmail(email, user.FirstName, user.LastName);
        }
    }
}
