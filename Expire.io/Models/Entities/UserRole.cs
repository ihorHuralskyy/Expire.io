using Microsoft.AspNetCore.Identity;

namespace UniversityProj.Models.Entities
{
    public class UserRole: IdentityUserRole<int>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
