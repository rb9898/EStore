using Microsoft.AspNetCore.Identity;

namespace Store.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
