using Expire.io.Models.Data;
using Expire.io.Models.Entities;
using Expire.io.Services.Contracts;
using Expire.io.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Threading.Tasks;

namespace Expire.io.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ExpireContext _context;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, ExpireContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<SignInResult> Login(LoginViewModel model)
        {
            var res = await _signInManager
                .PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            return res;
        }

        public async Task LogOff()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(RegisterViewModel model)
        {
            User user = new User()
            {
                FirstName = model.FName,
                LastName = model.LName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.Phone
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
            }
            return result;
        }
    }
}
