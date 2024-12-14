using BuildingBlocks.Messaging.Events;
using MassTransit;
using Odering.Application.Orders.Commands.CreateOrder;

namespace Odering.Application.Orders.EventHandlers.Integration
{
    public class CartCheckoutEventHandler(ISender sender, ILogger<CartCheckoutEventHandler> logger) : IConsumer<CartCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<CartCheckoutEvent> context)
        {
            logger.LogInformation("IntegrationEvent Handled: {IntegrationEvent}", context.Message.GetType().Name);

            var command = MapToCreateOrderCommand(context.Message);

            await sender.Send(command);
             
        }


        private CreateOrderCommand MapToCreateOrderCommand(CartCheckoutEvent message)
        {
            var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.Town);
            var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.ExpiryDate, message.CVV, message.PaymentMethod, message.PaymentRefNumber);
            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                Id: orderId,
                CustomerId: message.CustomerId,
                OrderName: message.UserName,
                DeliveryAddress: addressDto,
                CompanyAddress: addressDto,
                Payment: paymentDto,
                Status: Odering.Domain.Enums.OrderStatus.Pending,
                OrderItems:
                [
                    new OrderItemDto(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 5000),
                    new OrderItemDto(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 4000)
                ]);

            return new CreateOrderCommand(orderDto);
        }


    }
}
