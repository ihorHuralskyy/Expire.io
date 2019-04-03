using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<RoleDTO> Roles { get; set; }
        public UserImageDTO Image { get; set; }
        public int? ManagerId { get; set; }
        public ICollection<DocumentDTO> UserDocuments { get; set; }
    }
}
