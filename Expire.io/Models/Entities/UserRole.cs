using Microsoft.AspNetCore.Identity;

namespace Expire.io.Models.Entities
{
    public class UserRole: IdentityUserRole<int>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
