using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.Models.Entities;
using Newtonsoft.Json;
using Expire.io.ViewModels;
using System.IO;
using Expire.io.Models.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Expire.io.Helpers
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ExpireContext _context;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager, ExpireContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
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
                    UserName = "admin@gmail.com",
                    FirstName = "Sebastian",
                    LastName = "Laursen"
                };

                admin.Email = "admin@gmail.com";

                IdentityResult result = _userManager.CreateAsync(admin, "password").Result;

                if (result.Succeeded)
                {
                    admin = _userManager.FindByNameAsync("admin@gmail.com").Result;

                    _userManager.AddToRoleAsync(admin, "Admin").Wait();

                }
            }

        }

        public void CheckAndSend()
        {
            /*var list = _context.Documents.Where(item => (item.DateOfExpiry.Value - DateTime.Now).TotalDays < 0)
                .ToList();*/
            EmailSender sender = new EmailSender();
            sender.SendEmail("tarikkataryna1999@gmail.com","Taras","Kataryna");
        }
    }
}
