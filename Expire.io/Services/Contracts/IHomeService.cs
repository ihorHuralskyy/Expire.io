using Expire.io.DTOs;
using System.Threading.Tasks;

namespace Expire.io.Services.Contracts
{
    public interface IHomeService
    {
        Task<UserDTO> Index();
    }
}
