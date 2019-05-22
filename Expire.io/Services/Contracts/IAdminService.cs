using Expire.io.DTOs;
using Expire.io.Models.Entities;
using Expire.io.Services.Responses;
using Expire.io.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expire.io.Services.Contracts
{
    public interface IAdminService
    {
        Task<List<UserDTORolesList>> AllUsers();
        Task<ResponseForAdmin> CreateUser(AdminAddUserViewModel model);
        Task<ResponseForAdmin> DeleteUser(string username);
        ResponseForAdmin MakeUserAnAdmin(string username);
        ResponseForAdmin MakeUserAnManager(string username);
        List<string> GetManagerList();
        ManagerUser GiveToManager(string managerName, int userId);
    }
}
