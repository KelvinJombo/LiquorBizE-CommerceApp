using BuildingBlocks.CQRS;
using Odering.Application.Data;
using Odering.Application.Dtos;


namespace Odering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext context) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(command.Order);

            context.Orders.Add(order);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateOrderResult(order.Id.Value);
             
        }


        private Order CreateNewOrder(OrderDto orderDto)
        {
            var deliveryAddress = Address.Of(orderDto.DeliveryAddress.FirstName, orderDto.DeliveryAddress.LastName, orderDto.DeliveryAddress.EmailAddress, orderDto.DeliveryAddress.AddressLine, orderDto.DeliveryAddress.Country, orderDto.DeliveryAddress.State, orderDto.DeliveryAddress.Town);
            var companyAddress = Address.Of(orderDto.CompanyAddress.FirstName, orderDto.CompanyAddress.LastName, orderDto.CompanyAddress.EmailAddress, orderDto.CompanyAddress.AddressLine, orderDto.CompanyAddress.Country, orderDto.CompanyAddress.State, orderDto.CompanyAddress.Town);

            var newOrder = Order.Create(
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(orderDto.CustomerId),
                orderName: OrderName.Of(orderDto.OrderName),
                deliveryAddress: deliveryAddress,
                companyAddress: companyAddress,
                payment: Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.ExpiryDate, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod, orderDto.Payment.PaymentRefNumber)
                );

            foreach (var orderItemDto in orderDto.OrderItems)
            {
                newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
                               
            }
            return newOrder;
        }
    }
}
