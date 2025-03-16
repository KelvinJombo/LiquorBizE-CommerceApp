using System.Security.Claims;

namespace LiquorSales.Web.Pages
{
    public class OrderListModel(IOrderingService orderingService) : PageModel
    {
        public IEnumerable<OrderModel> Orders { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            // Get the logged-in user's CustomerId from claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(customerIdClaim) || !Guid.TryParse(customerIdClaim, out var customerId))
            {
                return Unauthorized(); // Return 401 if the user is not authenticated or CustomerId is invalid
            }

            var response = await orderingService.GetOrdersByCustomer(customerId);

            Orders = response.Orders;

            return Page();
        }

    }
}
