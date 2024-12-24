﻿namespace LiquorSales.Web.Services
{
    public interface ICatalogueServices
    {
        [Get("/catalogue-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]
        Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);

        [Get("/catalogue-service/products/{id}")]
        Task<GetProductByIdResponse> GetProduct(Guid id);

        [Get("/catalogue-service/products/category/{category}")]
        Task<GetProductByCategoryResponse> GetProductByCategory(string category);
    }
}
