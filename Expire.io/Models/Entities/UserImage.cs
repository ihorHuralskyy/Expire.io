using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityProj.Models.Entities
{
    public class UserImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
