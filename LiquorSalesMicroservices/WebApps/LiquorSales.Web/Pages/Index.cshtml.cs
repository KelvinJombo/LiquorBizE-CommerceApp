using LiquorSales.Web.Implementations;
using Microsoft.AspNetCore.Http;

namespace LiquorSales.Web.Pages
{
    public class IndexModel(ICatalogueServices catalogueServices, ILoadCartServices services, ICartServices cartServices, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
    {
        
        public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();
         
        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Index page visited");

            try
            {
                var result = await catalogueServices.GetProducts();

                // var result = await catalogueServices.GetProduct(2, 3);

                ProductList = result.Products;

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


        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {

            var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(); // User is not authenticated
            }

            logger.LogInformation("Add To Cart Button Clicked");

            try
            {
                var productResponse = await catalogueServices.GetProduct(productId);
                var cart = await services.LoadUserCart();

                cart.Items.Add(new ShoppingCartItemModel
                {
                    ProductId = productId,
                    ProductName = productResponse.Product.Name,
                    SellingPrice = productResponse.Product.SellingPrice,
                    Quantity = 1,
                    Size = productResponse.Product.Size,
                });

                await cartServices.StoreCart(new StoreCartRequest(cart), $"Bearer {token}");

                return RedirectToPage("Cart");

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
