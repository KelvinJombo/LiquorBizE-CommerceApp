using BuildingBlocks.Pagination;

namespace Odering.Application.Orders.Querries.GetOrders
{
    public record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersResult>;
    
    public record GetOrdersResult(PaginatedResult<OrderDto> Orders);
}
