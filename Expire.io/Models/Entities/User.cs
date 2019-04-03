using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UniversityProj.Models.Entities
{
    public class User: IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public UserImage Image { get; set; }
        public int? ManagerId { get; set; }
        public ICollection<UserDocument> UserDocuments { get; set; }
    }
}
