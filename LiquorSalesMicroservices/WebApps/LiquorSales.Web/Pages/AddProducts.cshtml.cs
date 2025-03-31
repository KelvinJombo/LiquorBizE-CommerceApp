using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace LiquorSales.Web.Pages
{
    public record CreateProductRequest(
        string Name, string CompanyName, string Size, List<string> Category,
        string Description, int StockingQuantity, string ImagePath,
        decimal CostPrice, decimal SellingPrice, DateOnly? ExpiryDate
    );

    public record CreateProductResponse(Guid Id);

    public class CreateProductModel(ICatalogueServices _catalogueServices, IWebHostEnvironment _environment, ILogger<CreateProductModel> _logger, IHttpContextAccessor httpContextAccessor) : PageModel
    {
        [BindProperty]
        public List<string> Category { get; set; } = new();

        [BindProperty]
        public CreateProductRequest Product { get; set; } = new(
            "", "", "", new List<string>(), "", 0, "default-image.jpg", 0m, 0m, null
        );

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var token = httpContext?.User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            // Ensure ExpiryDate has a valid value
            var expiryDate = Product.ExpiryDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1));

            if (expiryDate <= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                ModelState.AddModelError(nameof(Product.ExpiryDate), "Expiry date cannot be in the past.");
                return Page();
            }

            // Process categories correctly
            var categories = Category?.Select(c => c.Trim()).Where(c => !string.IsNullOrEmpty(c)).ToList() ?? new List<string>();

            string fileName = "default-image.jpg"; // Default image

            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(ImageFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError(nameof(ImageFile), "Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
                        return Page();
                    }

                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    fileName = $"{Guid.NewGuid()}_{Path.GetFileName(ImageFile.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var image = await Image.LoadAsync<Rgba32>(ImageFile.OpenReadStream()))
                    {
                        image.Mutate(x => x.Resize(200, 200));
                        await image.SaveAsync(filePath);
                    }
                }

                // Create a new immutable Product object with updated properties
                Product = Product with { Category = categories, ImagePath = fileName, ExpiryDate = expiryDate };

                ModelState.Remove("Product.Category");
                ModelState.Remove("Product.ImagePath");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogError("Validation failed: " + string.Join("; ", errors));

                    return Page();
                }

                var response = await _catalogueServices.CreateProduct(Product, $"Bearer {token}");

                if (response == null || response.Id == Guid.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create product. Please try again.");
                    return Page();
                }

                ViewData["SuccessMessage"] = "Product created successfully!";
                return Page();
            }
            catch (UnknownImageFormatException ex)
            {
                _logger.LogError(ex, "Invalid image format uploaded.");
                ModelState.AddModelError(nameof(ImageFile), "Invalid image format.");
                ViewData["ErrorMessage"] = "Invalid image format uploaded!";
                return Page();  // Ensure the page reloads with the error message
            }
            catch (ApiException ex)
            {
                _logger.LogError("API error: {Error}, Status Code: {StatusCode}", ex.Content, ex.StatusCode);
                ModelState.AddModelError(string.Empty, $"API Error: {ex.Content}");
                ViewData["ErrorMessage"] = "Server Error Encountered!";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating product.");
                ModelState.AddModelError(string.Empty, $"Unexpected Error: {ex.Message}");
                ViewData["ErrorMessage"] = "Unexpected error occurred while creating product!";
                return Page();
            }
             
        }
    }
}
