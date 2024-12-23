
namespace CatalogueApi.Products.UpdateProducts
{
    public record UpdateProductRequest(Guid Id, string Name, string CompanyName, List<string> Category, int StockingQuantity, string Description, string ImageFile, decimal CostPrice, decimal SellingPrice, DateOnly ExpiryDate);

    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
               //.RequireAuthorization("AdminUserOnly")
               .WithName("UpdateProduct")
               .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Update Product")
               .WithDescription("Update Product");
        }
    }
}
