using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Expires { get; set; }

        public string? Token { get; set; }
    }
}

