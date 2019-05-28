using Expire.io.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.Services.Contracts
{
    public interface IManagerService
    {
        List<UserForManagerDTO> AllUsersByManager();
        void Notify(string email);
    }
}
