namespace Odering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Order.Id);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);

            if (order is null)
            {
                throw new OrderNotFoundException(command.Order.Id);
            }

            UpdateOrderWithNewValues(order, command.Order);

            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }



        public void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
        {
            var updatedDeliveryAddress = Address.Of(orderDto.DeliveryAddress.FirstName, orderDto.DeliveryAddress.LastName, orderDto.DeliveryAddress.EmailAddress, orderDto.DeliveryAddress.AddressLine, orderDto.DeliveryAddress.Country, orderDto.DeliveryAddress.State, orderDto.DeliveryAddress.Town);
            var updatdeCompanyAddress = Address.Of(orderDto.CompanyAddress.FirstName, orderDto.CompanyAddress.LastName, orderDto.CompanyAddress.EmailAddress, orderDto.CompanyAddress.AddressLine, orderDto.CompanyAddress.Country, orderDto.CompanyAddress.State, orderDto.CompanyAddress.Town);
            var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.ExpiryDate, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod, orderDto.Payment.PaymentRefNumber);

            order.Update(
                orderName: OrderName.Of(orderDto.OrderName),
                deliveryAddress: updatedDeliveryAddress,
                companyAddress: updatdeCompanyAddress,
                payment: updatedPayment,
                status: orderDto.Status);
        }
    }   
}
