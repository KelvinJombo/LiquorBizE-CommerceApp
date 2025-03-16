
using Odering.Application.Orders.Commands.DeleteOrder;

namespace Odering.Api.EndPoints
{
    //public record DeleteOrderRequest(Guid Id);

    public record DeleteOrderResponse(bool IsSuccess);

    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(Id));

                var response = result.Adapt<DeleteOrderResponse>();

                return Results.Ok(response);

            })
                //.RequireAuthorization("AdminUserOrSuperAdminUserPolicy")
                .WithName("DeleteOrder")
                .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("DeleteOrder")
                .WithDescription("Delete Order");
        }
    }
}
