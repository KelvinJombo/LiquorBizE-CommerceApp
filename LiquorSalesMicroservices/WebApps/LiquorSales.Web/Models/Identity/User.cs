using Microsoft.AspNetCore.Identity;

namespace LiquorSales.Web.Models.Identity
{
    public class User : IdentityUser
    {
        public string Role { get; set; } = string.Empty;   
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
