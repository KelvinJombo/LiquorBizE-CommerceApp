namespace Odering.Application.Orders.Querries.GetOrdersByName
{
    public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;

    public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);


}
