using LiquorSales.Web.Implementations;

namespace LiquorSales.Web.Pages
{
    public class ProductListModel(ICatalogueServices catalogueServices, ILoadCartServices services, ILogger<ProductListModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
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
            try
            {
                var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                logger.LogInformation("Add To Cart Button Clicked");

                var productResponse = await catalogueServices.GetProduct(productId);
                var product = productResponse.Product;

                await services.AddToCart(
                    product.Id,
                    product.Name,
                    product.SellingPrice,
                    1,
                    product.Size,
                    product.Category);

                return RedirectToPage("Cart");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add product to cart");
                return RedirectToPage("Error");
            }

        }



    }
}
