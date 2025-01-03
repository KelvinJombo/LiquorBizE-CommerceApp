namespace Cart.Api.ShoppingCart.GetCart
{
    public record GetCartRequest(string UserName);

    public record GetCartResponse(ShoppingCarts Cart);
    public class GetCartEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/cart/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetCartQuery(userName));

                var response = result.Adapt<GetCartResponse>();

                return Results.Ok(response);
            })
                .RequireAuthorization("AdminUserPolicy")
                .WithName("GetCart By Id")
                .Produces<GetCartResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Cart By Id")
                .WithDescription("Get Cart By Id");
        }
    }
}
