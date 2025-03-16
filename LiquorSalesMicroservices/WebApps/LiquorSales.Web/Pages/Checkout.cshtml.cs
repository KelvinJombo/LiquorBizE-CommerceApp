using LiquorSales.Web.Implementations;
using System.Security.Claims;

namespace LiquorSales.Web.Pages
{
    public class CheckoutModel(ICartServices cartServices, ILoadCartServices services, ILogger<CheckoutModel> logger) : PageModel
    {
        [BindProperty]
        public CartCheckoutModel Order { get; set; } = default!;
        public ShoppingCartModel Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Cart = await services.LoadUserCart();

                return Page();
            }
            catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error occurred.");
                return new RedirectToPageResult("/Error", new { statusCode = (int)apiEx.StatusCode, errorMessage = apiEx.Content });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred.");
                return new RedirectToPageResult("/Error", new { statusCode = 500, errorMessage = "An unexpected error occurred." });
            }

        }


        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            logger.LogInformation("Checkout Button Clicked");

            try
            {
                Cart = await services.LoadUserCart();

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Get the logged-in user's details
                var user = HttpContext.User;
                if (user?.Identity?.IsAuthenticated != true)
                {
                    logger.LogWarning("Unauthorized checkout attempt.");
                    return RedirectToPage("/Login");
                }

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = user.Identity.Name;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
                {
                    logger.LogWarning("User ID or Username not found in claims.");
                    return RedirectToPage("/Error", new { statusCode = 400, errorMessage = "Invalid user session." });
                }

                Order.CustomerId = userId;
                Order.UserName = userName;
                Order.TotalPrice = Cart.TotalPrice;

                await cartServices.CheckoutCart(new CheckoutCartRequest(Order));

                return RedirectToPage("Confirmation", "OrderSubmitted");
            }
            catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error occurred.");
                return new RedirectToPageResult("/Error", new { statusCode = (int)apiEx.StatusCode, errorMessage = apiEx.Content });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred.");
                return new RedirectToPageResult("/Error", new { statusCode = 500, errorMessage = "An unexpected error occurred." });
            }
        }

    }
}
