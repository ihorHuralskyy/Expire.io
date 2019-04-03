using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.Models.Entities;
using Newtonsoft.Json;
using Expire.io.ViewModels;
using System.IO;

namespace Expire.io.Helpers
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory().Split("\\bin\\Debug").First() + "\\Helpers", "UserSeedData.json"));
                var deserializeObjects = JsonConvert.DeserializeObject<List<RegisterViewModel>>(userData);

                var list = new List<User>();
                foreach (var item in deserializeObjects)
                {
                    list.Add(Mappers.Mappers.FromRegViewModToUser(item));
                }

                var roles = new List<Role>
                {
                    new Role{ Name = "Admin"},
                    new Role{ Name = "Manager"},
                    new Role{ Name = "User" }
                };

                foreach (var item in roles)
                {
                    _roleManager.CreateAsync(item).Wait();
                }

                for (int i = 0; i < list.Count; i++)
                {
                    var res = _userManager.CreateAsync(list[i], deserializeObjects[i].Password).Result;
                    if (res.Succeeded)
                        _userManager.AddToRoleAsync(list[i], "Manager").Wait();
                }

                var admin = new User
                {
                    UserName = "Admin",
                    FirstName = "Sebastian",
                    LastName = "Laursen"
                };

                IdentityResult result = _userManager.CreateAsync(admin, "password").Result;

                if (result.Succeeded)
                {
                    admin = _userManager.FindByNameAsync("Admin").Result;

                    _userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }

        }
    }
}
