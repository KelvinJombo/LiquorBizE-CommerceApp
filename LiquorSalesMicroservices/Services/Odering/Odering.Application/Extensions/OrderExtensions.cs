﻿namespace Odering.Application.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
        {


            return orders.Select(order => new OrderDto(
                Id: order.Id.Value,
                CustomerId: order.CustomerId.Value,
                OrderName: order.OrderName.Value,
                DeliveryAddress: new AddressDto(
                    order.DeliveryAddress.FirstName,
                    order.DeliveryAddress.LastName,
                    order.DeliveryAddress.EmailAddress,
                    order.DeliveryAddress.AddressLine,
                    order.DeliveryAddress.Country,
                    order.DeliveryAddress.State,
                    order.DeliveryAddress.Town),
                CompanyAddress: new AddressDto(
                    order.CompanyAddress.FirstName,
                    order.CompanyAddress.LastName,
                    order.CompanyAddress.EmailAddress,
                    order.CompanyAddress.AddressLine,
                    order.CompanyAddress.Country,
                    order.CompanyAddress.State,
                    order.CompanyAddress.Town),
                Payment: new PaymentDto(
                    order.Payment.CardName,
                    order.Payment.CardNumber,
                    order.Payment.ExpiryDate,
                    order.Payment.CVV,
                    order.Payment.PaymentMethod,
                    order.Payment.PaymentRefNumber),
                Status: order.Status,
                OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
            ));

        }       
            


        public static OrderDto ToOrderDto(this Order order)
        {
            return DtoFromOrder(order);
        }

        private static OrderDto DtoFromOrder(Order order)
        {
            return new OrderDto(
                Id: order.Id.Value,
                CustomerId: order.CustomerId.Value,
                OrderName: order.OrderName.Value,
                DeliveryAddress: new AddressDto(order.DeliveryAddress.FirstName, order.DeliveryAddress.LastName, order.DeliveryAddress.EmailAddress, order.DeliveryAddress.AddressLine, order.DeliveryAddress.Country, order.DeliveryAddress.State, order.DeliveryAddress.Town),
                CompanyAddress: new AddressDto(order.CompanyAddress.FirstName, order.CompanyAddress.LastName, order.CompanyAddress.EmailAddress, order.CompanyAddress.AddressLine, order.CompanyAddress.Country, order.CompanyAddress.State, order.CompanyAddress.Town),
                Payment: new PaymentDto(order.Payment.CardName, order.Payment.CardNumber, order.Payment.ExpiryDate, order.Payment.CVV, order.Payment.PaymentMethod, order.Payment.PaymentRefNumber),
                Status: order.Status,
                OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()
                );
        }
    }
}
