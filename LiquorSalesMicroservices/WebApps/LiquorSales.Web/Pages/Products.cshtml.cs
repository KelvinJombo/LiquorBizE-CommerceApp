using Microsoft.AspNetCore.Http;

namespace LiquorSales.Web.Pages
{
    public class ProductsModel(ICatalogueServices _catalogueServices, IHttpContextAccessor httpContextAccessor) : PageModel
    {


        public IEnumerable<ProductModel> Products { get; set; } = new List<ProductModel>();

        public string SearchQuery { get; set; } = "";

        public async Task OnGetAsync(string? search = "")
        {
            SearchQuery = search;
            var response = await _catalogueServices.GetProducts();
            Products = response.Products;

            if (!string.IsNullOrEmpty(search))
            {
                Products = Products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(); // User is not authenticated
            }

            await _catalogueServices.DeleteProduct(id, $"Bearer {token}");
            return RedirectToPage();
        }
    }
}

