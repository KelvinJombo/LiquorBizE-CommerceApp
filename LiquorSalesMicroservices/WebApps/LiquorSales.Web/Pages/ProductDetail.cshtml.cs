using LiquorSales.Web.Implementations;

namespace LiquorSales.Web.Pages.Shared
{
    public class ProductDetailModel(ICatalogueServices catalogueServices, ICartServices cartServices, ILoadCartServices services, ILogger<ProductDetailModel> logger) : PageModel
    {
        public ProductModel Product { get; set; } = default!;

        [BindProperty]
        public string Size { get; set; } = default!;

        [BindProperty]
        public int Quantity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid productId)
        {
            var response = await catalogueServices.GetProduct(productId);

            Product = response.Product;

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            logger.LogInformation("Add To Cart Button Clicked");

            var productResponse = await catalogueServices.GetProduct(productId);

            var cart = await services.LoadUserCart();

            cart.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Product.Name,
                SellingPrice = productResponse.Product.SellingPrice,
                Quantity = Quantity,
                Size = productResponse.Product.Size,
            });

            await cartServices.StoreCart(new StoreCartRequest(cart));

            return RedirectToPage("Cart");
        }

    }
} 
