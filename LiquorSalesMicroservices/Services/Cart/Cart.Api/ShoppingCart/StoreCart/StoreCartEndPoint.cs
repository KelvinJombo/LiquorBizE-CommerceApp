
namespace Cart.Api.ShoppingCart.StoreCart
{
    public record StoreCartRequest(ShoppingCarts Cart);
    public record StoreCartResponse(string UserName);
    public class StoreCartEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/cart", async (StoreCartRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreCartCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<StoreCartResponse>();

                return Results.Created($"/cart/{response.UserName}", response);
            })
               .RequireAuthorization("RegulaUserPolicy")
               .WithName("Create Cart")
               .Produces<StoreCartResponse>(StatusCodes.Status201Created)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Create Cart")
               .WithDescription("Create Cart");
        }
    }
}
