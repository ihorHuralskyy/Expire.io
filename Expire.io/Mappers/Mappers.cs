using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expire.io.Models.Entities;
using Expire.io.ViewModels;

namespace Expire.io.Mappers
{
    public static class Mappers
    {
        public static User FromRegViewModToUser(RegisterViewModel model)
        {
            return new User
            {
                FirstName = model.FName,
                LastName = model.LName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email
            };
        }


    }
}
