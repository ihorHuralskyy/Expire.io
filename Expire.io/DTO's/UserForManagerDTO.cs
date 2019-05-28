using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Expire.io.DTOs
{
    public class UserForManagerDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}
