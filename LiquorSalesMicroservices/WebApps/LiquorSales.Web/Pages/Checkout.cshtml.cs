namespace LiquorSales.Web.Pages
{
    public class CheckoutModel(ICartServices cartServices, ILogger<CheckoutModel> logger) : PageModel
    {
        [BindProperty]
        public CartCheckoutModel Order { get; set; } = default!;
        public ShoppingCartModel Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Cart = await cartServices.LoadUserCart();

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
                Cart = await cartServices.LoadUserCart();

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Assuming CustomerId is Passed In From The UI

                Order.CustomerId = "58c49479-ec65-4de2-86e7-033c546291aa";
                Order.UserName = Cart.UserName;
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
