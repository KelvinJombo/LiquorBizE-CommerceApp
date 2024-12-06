using Odering.Domain.Enums;

namespace Odering.Application.Dtos
{
     public record OrderDto( Guid Id, Guid CustomerId, string OrderName, AddressDto DeliveryAddress, AddressDto CompanyAddress, PaymentDto Payment, OrderStatus Status, List<OrderItemDto> OrderItems);
}
