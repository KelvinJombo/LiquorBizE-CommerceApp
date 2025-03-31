using LiquorSales.Web.Implementations;
using Microsoft.AspNetCore.Http;

namespace LiquorSales.Web.Pages
{
    public class ProductListModel(ICatalogueServices catalogueServices, ICartServices cartServices, ILoadCartServices services, ILogger<ProductListModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
    {
        public IEnumerable<string> CategoryList { get; set; } = [];

        public IEnumerable<ProductModel> ProductList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var response = await catalogueServices.GetProducts();

            CategoryList = response.Products.SelectMany(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                ProductList = response.Products.Where(p => p.Category.Contains(categoryName));
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = response.Products;
            }

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

    }
}
