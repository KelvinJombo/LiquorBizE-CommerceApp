using Cart.Api.ShoppingCart.StoreCart;

namespace Cart.Api.ShoppingCart.UpdateCart
{
    public class UpdateCartEndpoint : ICarterModule
    {
        public record UpdateCartRequest(ShoppingCarts Cart);
        public record UpdateCartResponse(string UserName);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/cart", async (UpdateCartRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateCartCommand>();  

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateCartResponse>();

                return Results.Ok(response); // 200 OK = valid for updates
            })
            //.RequireAuthorization("RegulaUserPolicy")
            .WithName("Update Cart")
            .Produces<UpdateCartResponse>(StatusCodes.Status200OK)  
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Cart")
            .WithDescription("Updates an existing Shopping Cart");
        }

    }
}
