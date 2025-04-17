using LiquorSales.Web.Implementations;

namespace LiquorSales.Web.Pages
{
    public class CartModel(ICartServices cartServices, ILoadCartServices services, ILogger<CartModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
    {

        [BindProperty]
        public UpdateCartRequest UpdateRequest { get; set; }

        public ShoppingCartModel Cart {  get; set; } = new ShoppingCartModel();


        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Cart = await services.LoadUserCart();

                if (Cart.Items is { Count: > 0 })
                {
                    foreach (var item in Cart.Items)
                    {
                        logger.LogInformation("Item: {ProductName}, Price: {SellingPrice}, Quantity: {Quantity}",
                            item.ProductName, item.SellingPrice, item.Quantity);
                    }
                }
                else
                {
                    logger.LogInformation("Cart is empty for user: {UserName}", Cart.UserName);
                }

                logger.LogInformation("Cart total for {UserName}: {TotalPrice}", Cart.UserName, Cart.TotalPrice);

                return Page();
            }
            catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error occurred while loading the cart for user.");
                return RedirectToPage("/Error", new { statusCode = (int)apiEx.StatusCode, errorMessage = apiEx.Content });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occurred while loading the cart.");
                return RedirectToPage("/Error", new { statusCode = 500, errorMessage = "An unexpected error occurred." });
            }
        }



        public async Task<IActionResult> OnPostRemoveFromCartAsync(Guid productId)
        {
            logger.LogInformation("RemoveFromCart Button Clicked");

            try
            {
                var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(); // User is not authenticated
                }


                Cart = await services.LoadUserCart();

                Cart.Items.RemoveAll(x => x.ProductId == productId);

                await cartServices.StoreCart(new StoreCartRequest(Cart), $"Bearer {token}");

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



        //public async Task<IActionResult> OnPostUpdateQuantityAsync()
        //{
        //    try
        //    {
        //        var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
        //        if (string.IsNullOrEmpty(token))
        //        {
        //            return Unauthorized();
        //        }

        //        var productId = UpdateRequest.ProductId;
        //        var quantity = UpdateRequest.Quantity;

        //        Cart = await services.LoadUserCart();

        //        var item = Cart.Items.FirstOrDefault(x => x.ProductId == productId);
        //        if (item != null)
        //        {
        //            item.Quantity = quantity;
        //            await cartServices.StoreCart(new StoreCartRequest(Cart), $"Bearer {token}");
        //        }

        //        return new JsonResult(new { success = true, newTotalPrice = Cart.TotalPrice });
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, "Error updating cart quantity.");
        //        return new JsonResult(new { success = false, message = "Error updating cart." });
        //    }
        //}


    }
}
