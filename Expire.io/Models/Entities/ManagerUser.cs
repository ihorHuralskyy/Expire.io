using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.Models.Entities
{
    public class ManagerUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }
        public User User { get; set; }
        public User Manager { get; set; }

    }
}
