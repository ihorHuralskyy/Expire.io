using Expire.io.DTOs;
using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.Services.Contracts;
using Expire.io.Services.Responses;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly ExpireContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AdminService(UserManager<User> userManager, 
            ExpireContext context, SignInManager<User> signInManager, 
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDTORolesList>> AllUsers()
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
                item.Roles = await _userManager
                    .GetRolesAsync(_userManager.Users.Where(u => u.UserName == item.UserName).First());
            }
            return allUsers.ToList();
        }        

        public async Task<ResponseForAdmin> DeleteUser(string username)
        {
            var user = _userManager.Users.Where(u => u.UserName == username).First();
            int Id = user.Id;
            var result = await _userManager.DeleteAsync(user);
            return new ResponseForAdmin
            {
                User = user,
                Result = result
            };
        }

        public async Task<ResponseForAdmin> CreateUser(AdminAddUserViewModel model)
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
            }
            return new ResponseForAdmin
            {
                User = user,
                Result = result
            };
        }

        public List<string> GetManagerList()
        {
            int roleId = _roleManager.Roles.FirstOrDefault(item => item.Name == "manager").Id;

            var list = _context.UserRoles
                .Where(item => item.RoleId == roleId).Select(u => _userManager.Users
                .FirstOrDefault(uu => uu.Id == u.UserId).UserName).ToList();

            return list;
        }

        public ManagerUser GiveToManager(string managerName, int userId)
        {
            int managerId = _userManager.Users.FirstOrDefault(item => item.UserName == managerName).Id;

            var res = _context.ManagerUsers.FirstOrDefault(item =>
                item.ManagerId == managerId && item.UserId == userId);

            if(res == null)
            {
                ManagerUser managerUser = new ManagerUser
                {
                    UserId = userId,
                    ManagerId = managerId
                };

                _context.ManagerUsers.Add(managerUser);
                _context.SaveChanges();
            }

            return res;
        }

        public ResponseForAdmin MakeUserAnAdmin(string username)
        {
            var user = _userManager.Users.Where(u => u.UserName == username).First();
            int Id = user.Id;
            var result = _userManager.AddToRoleAsync(user, "Admin").Result;
            return new ResponseForAdmin
            {
                User = user,
                Result = result
            };
        }

        public ResponseForAdmin MakeUserAnManager(string username)
        {
            var user = _userManager.Users.Where(u => u.UserName == username).First();
            int Id = user.Id;
            var result = _userManager.AddToRoleAsync(user, "Manager").Result;
            return new ResponseForAdmin
            {
                User = user,
                Result = result
            };
        }
    }
}
