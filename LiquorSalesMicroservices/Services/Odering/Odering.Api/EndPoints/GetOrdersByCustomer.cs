
using Odering.Application.Orders.Querries.GetOrdersByCustomer;

namespace Odering.Api.EndPoints
{
    //public record GetOrdersByCustomerRequest(Guid CustomerId);

    public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

    public class GetOrdersByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapGet("/orders/customer/{customerId}", async (Guid CustomerId, ISender sender) =>
           {
               var result = await sender.Send(new GetOrdersByCustomerQuery(CustomerId));

               var response = result.Adapt<GetOrdersByCustomerResponse>();

               return Results.Ok(response);
           })

                .WithName("GetOrdersByCustomer")
                .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("GetOrdersByCustomer")
                .WithDescription("Get Orders By Customer");
        }
    }
}
