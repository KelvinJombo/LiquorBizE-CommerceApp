using LiquorSales.Web.Pages;

namespace LiquorSales.Web.Services
{
    public interface ICatalogueServices
    {
        [Get("/catalogue-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]
        Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);

        [Get("/catalogue-service/products/{id}")]
        Task<GetProductByIdResponse> GetProduct(Guid id);

        [Get("/catalogue-service/products/category/{category}")]
        Task<GetProductByCategoryResponse> GetProductByCategory(string category);

        [Get("/catalogue-service/products/{id}")]
        Task<GetProductByIdResponse> DeleteProduct(Guid id, [Header("Authorization")] string authorization);

        [Post("/catalogue-service/products")]
        Task<CreateProductResponse> CreateProduct([Body] CreateProductRequest request, [Header("Authorization")] string authorization);

        [Put("/catalogue-service/products")]
        Task<CreateProductResponse> UpdateProduct([Body] UpdateProductRequest request, [Header("Authorization")] string authorization);
    }
}
