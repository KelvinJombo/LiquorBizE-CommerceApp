namespace CatalogueApi.Products.CreateProduct
{
    public record CreateProductRequest(string Name, string CompanyName, string Size, List<string> Category, string Description, int StockingQuantity, string ImagePath, decimal CostPrice, decimal SellingPrice, DateOnly ExpiryDate);
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.Id}", response);
            })
                //.RequireAuthorization("AdminUserOnly")
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");

        }
    }
}
