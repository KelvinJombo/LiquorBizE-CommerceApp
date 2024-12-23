namespace Cart.Api.ShoppingCart.CheckoutCart
{
    public record CheckoutCartRequest(CartCheckoutDto CartCheckoutDto);

    public record CheckoutCartResponse(bool IsSuccess);

    public class CheckoutCartEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/cart/checkout", async (CheckoutCartRequest request, ISender sender) =>
            {
                var command = request.Adapt<CheckoutCartCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CheckoutCartResponse>();

                return Results.Ok(response);
            })
                //.RequireAuthorization("AdminUserOnly")
                .WithName("CheckoutCart")
                .Produces<CheckoutCartResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Checkout Cart")
                .WithDescription("Checkout Cart");
        }
    }
}
