using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Expire.io.DTOs;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;


namespace Expire.io.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ExpireContext _context;
        public HomeController(UserManager<User> userManager, ExpireContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var roles = await _userManager.GetRolesAsync(user);

            UserImage img = null;
            try
            {
               img = await _context.UserImages.FirstAsync(i => i.UserId == user.Id);
            }
            catch (Exception e)
            {
            }

            var userDTO = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id,
                Phone = user.PhoneNumber,
                ManagerId = user.ManagerId,
                UserName = user.UserName,
                Roles = roles.Select(r => new RoleDTO() { Name = r }).ToList()
            };
            if (img != null)
                userDTO.Image = new UserImageDTO { Id = img.Id, Image = img.Image, UserId = userDTO.Id };
            return View(userDTO);
        }

    }
}
