using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UniversityProj.Models.Entities
{
    public class Role: IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
