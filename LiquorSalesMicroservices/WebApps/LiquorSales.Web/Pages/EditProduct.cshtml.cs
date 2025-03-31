using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LiquorSales.Web.Pages
{
    public record UpdateProductRequest(Guid Id, string Name, string CompanyName, string Size,
        List<string> Category, string Description, string ImagePath, decimal SellingPrice, int StockingQuantity, DateOnly? ExpiryDate);



    public class EditProductModel(ICatalogueServices _catalogueServices, IWebHostEnvironment _environment, IHttpContextAccessor httpContextAccessor, ILogger<EditProductModel> _logger) : PageModel
    {        
        [BindProperty]
        public UpdateProductRequest Product { get; set; } = new(Guid.Empty, "", "", "", new List<string>(), "", "", 0m, 0, DateOnly.FromDateTime(DateTime.UtcNow));

        [BindProperty]
        public string CategoryInput { get; set; } = string.Empty;

        [BindProperty]
        public List<string> Category { get; set; } = new();


        [BindProperty]
        public IFormFile? ImageFile { get; set; }  

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var response = await _catalogueServices.GetProduct(id);
            if (response == null || response.Product == null)
            {
                return NotFound();
            }

            var product = response.Product;

            Product = new UpdateProductRequest(
                product.Id,
                product.Name,
                product.CompanyName,
                product.Size,
                product.Category,
                product.Description,
                product.ImagePath,
                product.SellingPrice,
                product.StockingQuantity,
                product.ExpiryDate
            );

            CategoryInput = string.Join(", ", product.Category ?? new List<string>());

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var token = httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                var expiryDate = Product.ExpiryDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1));

                if (expiryDate <= DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    ModelState.AddModelError(nameof(Product.ExpiryDate), "Expiry date cannot be in the past.");
                    return Page();
                }

                Category = (CategoryInput ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList();
                Product = Product with { Category = Category };
                

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    await HandleImageUploadAsync();
                }               


                ModelState.Remove("Product.Category");
                ModelState.Remove("Product.ImagePath");

                if (!ModelState.IsValid)
                {
                    LogValidationErrors();
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(Product.Name) || string.IsNullOrWhiteSpace(Product.CompanyName) ||
                    string.IsNullOrWhiteSpace(Product.Size) ||
                    Product.StockingQuantity <= 0 || Product.SellingPrice <= 0)
                {
                    ModelState.AddModelError(string.Empty, "All required fields must be filled properly.");
                    return Page();
                }

                await _catalogueServices.UpdateProduct(Product, $"Bearer {token}");
                ViewData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToPage("Products");
            }
            catch (ApiException ex)
            {
                var errorContent = await ex.GetContentAsAsync<string>(); 
                _logger.LogError("API error: {Error}, Status Code: {StatusCode}, Response: {Response}",
                                  ex.Content, ex.StatusCode, errorContent);
                ModelState.AddModelError(string.Empty, $"API Error: {errorContent}");
                ViewData["ErrorMessage"] = "Server Error Encountered!";
                return Page();
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "File system error occurred.");
                ModelState.AddModelError(string.Empty, "Error handling image file.");
                ViewData["ErrorMessage"] = "File system error occurred!";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating product.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
                ViewData["ErrorMessage"] = "Unexpected error occurred while updating product!";
                return Page();
            }
             
        }



        private async Task HandleImageUploadAsync()
        {
            try
            {
                ModelState.Remove("ImageFile");

                if (ImageFile == null || ImageFile.Length == 0)
                {
                    _logger.LogWarning("No image file uploaded.");
                    return;
                }

                // Delete old file if exists
                if (!string.IsNullOrEmpty(Product.ImagePath))
                {
                    string oldFilePath = Path.Combine(_environment.WebRootPath, "uploads", Product.ImagePath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string newFileName = $"{Guid.NewGuid()}_{Path.GetFileName(ImageFile.FileName)}";
                string newFilePath = Path.Combine(uploadsFolder, newFileName);

                using (var image = await Image.LoadAsync<Rgba32>(ImageFile.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(200, 200)); 
                    await image.SaveAsync(newFilePath);
                }

                // Assign correct image path
                Product = Product with { ImagePath = newFileName };
            }
            catch (UnknownImageFormatException ex)
            {
                _logger.LogError(ex, "Invalid image format uploaded.");
                ModelState.AddModelError("ImageFile", "Invalid image format.");
            }
        }


        private void LogValidationErrors()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            _logger.LogError("Validation failed: {Errors}", string.Join("; ", errors));
        }




    }
}
