using Microsoft.AspNetCore.Identity;

namespace assets.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string JobTitle { get; set; }
        public Department Department { get; set; }

    }
}