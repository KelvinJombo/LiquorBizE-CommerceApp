using LiquorSales.Web.Implementations;
using Microsoft.AspNetCore.Http;

namespace LiquorSales.Web.Pages.Shared
{
    public class ProductDetailModel(ICatalogueServices catalogueServices, IHttpContextAccessor httpContextAccessor, ICartServices cartServices, ILoadCartServices services, ILogger<ProductDetailModel> logger) : PageModel
    {
        public ProductModel Product { get; set; } = default!;

        [BindProperty]
        public string Size { get; set; } = default!;

        [BindProperty]
        public int Quantity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid productId)
        {
            var response = await catalogueServices.GetProduct(productId);

            if (response == null || response.Product == null)
            {
                return NotFound();   
            }

            Product = response.Product;
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(); // User is not authenticated
            }

            logger.LogInformation("Add To Cart Button Clicked");

            var productResponse = await catalogueServices.GetProduct(productId);

            if (productResponse == null || productResponse.Product == null)
            {
                logger.LogError("Product not found for ID: {ProductId}", productId);
                return NotFound(); // Prevents null reference exception
            }

            var cart = await services.LoadUserCart();

            cart.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Product.Name,
                SellingPrice = productResponse.Product.SellingPrice,
                Quantity = Quantity,
                Size = productResponse.Product.Size,
            });

            await cartServices.StoreCart(new StoreCartRequest(cart), $"Bearer {token}");

            return RedirectToPage("Cart");
        }


    }
} 
