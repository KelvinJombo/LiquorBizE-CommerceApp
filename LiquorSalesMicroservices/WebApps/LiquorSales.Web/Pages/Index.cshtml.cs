namespace LiquorSales.Web.Pages
{
    public class IndexModel(ICatalogueServices catalogueServices, ILogger<IndexModel> logger) : PageModel
    {
        
        public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();
         
        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Index page visited");

            var result = await catalogueServices.GetProducts();

            ProductList = result.Products;

            return Page();
        }

        
    }
}
