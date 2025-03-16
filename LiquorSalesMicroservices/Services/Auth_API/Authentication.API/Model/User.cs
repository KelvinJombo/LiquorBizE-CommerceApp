using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Model
{
    public class User : IdentityUser
    {
        public string Role { get; set; } = string.Empty;   
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
