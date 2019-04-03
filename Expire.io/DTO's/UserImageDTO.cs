using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.DTOs
{
    public class UserImageDTO
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int? UserId { get; set; }
    }
}
