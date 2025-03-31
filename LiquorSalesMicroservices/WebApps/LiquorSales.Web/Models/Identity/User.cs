using Microsoft.AspNetCore.Identity;

namespace LiquorSales.Web.Models.Identity
{
    public class User : IdentityUser
    {
        public string CustomerId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;   
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
