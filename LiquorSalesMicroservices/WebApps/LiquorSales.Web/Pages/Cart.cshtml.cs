namespace LiquorSales.Web.Pages
{
    public class CartModel(ICartServices cartServices, ILogger<CartModel> logger) : PageModel
    {
        public ShoppingCartModel Cart {  get; set; } = new ShoppingCartModel();
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


        public async Task<IActionResult> OnPostRemoveFromCartAsync(Guid productId)
        {
            logger.LogInformation("RemoveFromCart Button Clicked");

            try
            {
                Cart = await cartServices.LoadUserCart();

                Cart.Items.RemoveAll(x => x.ProductId == productId);

                await cartServices.StoreCart(new StoreCartRequest(Cart));

                return RedirectToPage();
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
