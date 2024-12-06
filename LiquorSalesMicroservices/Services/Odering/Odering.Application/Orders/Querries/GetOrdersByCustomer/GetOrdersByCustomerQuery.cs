namespace Odering.Application.Orders.Querries.GetOrdersByCustomer
{
    public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;

    public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders); 
     
}
