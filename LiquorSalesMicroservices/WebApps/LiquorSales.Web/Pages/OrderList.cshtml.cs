namespace LiquorSales.Web.Pages
{
    public class OrderListModel(IOrderingService orderingService, IHttpContextAccessor httpContextAccessor) : PageModel
    {
        public List<OrderModel> Orders { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
             
            var claims = httpContextAccessor.HttpContext?.User.Claims;             

            var customerIdClaim = httpContextAccessor.HttpContext?.User.FindFirst("CustomerId")?.Value;

            if (string.IsNullOrEmpty(customerIdClaim) || !Guid.TryParse(customerIdClaim, out var customerId))
            {
                return Unauthorized();  
            }

            var response = await orderingService.GetOrdersByCustomer(customerId, $"Bearer {token}");
            Orders = response?.Orders ?? new List<OrderModel>();

            return Page();
        }
    }

}
