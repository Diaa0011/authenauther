using Microsoft.AspNetCore.Identity;

namespace AuthenAutherApp.Models.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
    }
}