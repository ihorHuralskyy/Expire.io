using Expire.io.DTOs;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.Services
{
    public class HomeService : IHomeService
    {
        private readonly UserManager<User> _userManager;
        private readonly ExpireContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeService(UserManager<User> userManager, 
            ExpireContext context, 
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDTO> Index()
        {
            var user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

            var roles = await _userManager.GetRolesAsync(user);

            UserImage img = null;

            img = await _context.UserImages.FirstOrDefaultAsync(i => i.UserId == user.Id);

            var userDTO = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id,
                Phone = user.PhoneNumber,
                UserName = user.UserName,
                Roles = roles.Select(r => new RoleDTO() { Name = r }).ToList()
            };
            if (img != null)
                userDTO.Image = new UserImageDTO { Id = img.Id, Image = img.Image, UserId = userDTO.Id };

            return userDTO;
        }
    }
}
