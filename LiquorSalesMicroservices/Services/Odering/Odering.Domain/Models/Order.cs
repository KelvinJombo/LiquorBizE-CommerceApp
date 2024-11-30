using Odering.Domain.Events;

namespace Odering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public CustomerId CustomerId { get; private set; } = default!;
        public OrderName OrderName { get; private set; } = default!;
        public Address DeliveryAddress { get; private set; } = default!;
        public Address CompanyAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }


        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address deliveryAddress, Address companyAddress, Payment payment)
        {
            var order = new Order()
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                DeliveryAddress = deliveryAddress,
                CompanyAddress = companyAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };

            order.AddDomainEvent(new OrderCreatedEvent(order));

            return order;
        }


        public void Update(OrderName orderName, Address deliveryAddress, Address companyAddress, Payment payment, OrderStatus status)
        {
            OrderName = orderName;
            DeliveryAddress = deliveryAddress;
            CompanyAddress = companyAddress;
            Payment = payment;
            Status = status;

            AddDomainEvent(new OrderUpdatedEvent(this));
        }


        public void Add(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem = new OrderItem(Id, productId, quantity, price);

            _orderItems.Add(orderItem);
        }


        public void Remove(ProductId productId)
        {
            var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem is not null)
            {
                _orderItems.Remove(orderItem);
            }
        }

    }
}
