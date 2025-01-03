using LiquorSales.Web.Models.Ordering;

namespace LiquorSales.Web.Models.Ordering
{
    public record OrderModel(Guid Id, Guid CustomerId, string OrderName, AddressModel DeliveryAddress, AddressModel CompanyAddress, PaymentModel Payment, OrderStatus Status, List<OrderItemModel> OrderItems);
}

public record OrderItemModel(Guid OrderId, Guid ProductId, int Quantity, decimal SellingPrice);
public record AddressModel(string FirstName, string LastName, string EmailAddress, string AddressLine, string Country, string State, string Town);
public record PaymentModel(string CardName, string CardNumber, string ExpiryDate, string Cvv, int PaymentMethod, string PaymentRefNumber);




public enum OrderStatus
{
    Draft = 1,
    Pending = 2,
    Completed = 3,
    Cancelled = 4
}

// wrapper classes

public record GetOrdersResponse(PaginatedResult<OrderModel> Orders);
public record GetOrdersByNameResponse(IEnumerable<OrderModel> Orders);
public record GetOrdersByCustomerResponse(IEnumerable<OrderModel> Orders);
