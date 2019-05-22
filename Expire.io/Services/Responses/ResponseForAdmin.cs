using Expire.io.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Expire.io.Services.Responses
{
    public class ResponseForAdmin
    {
        public IdentityResult Result { get; set; }
        public User User { get; set; }
    }
}
