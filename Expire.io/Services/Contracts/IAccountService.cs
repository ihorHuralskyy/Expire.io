using Expire.io.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Expire.io.Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> Register(RegisterViewModel model);
        Task<SignInResult> Login(LoginViewModel model);
        Task LogOff();
    }
}
