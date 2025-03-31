namespace LiquorSales.Web.Services
{
    public interface IOrderingService
    {
        [Get("/ordering-service/orders?pageIndex={pageIndex}&pageSize={pageSize}\"")]
        Task<GetOrdersResponse> GetOrders([Header("Authorization")] string authorization, int? pageIndex = 1, int? pageSize =10);

        
        [Get("/ordering-service/orders/{orderName}")]
        Task<GetOrdersByNameResponse> GetOrdersByName(string orderName, [Header("Authorization")] string authorization);

       
        [Get("/ordering-service/orders/customer/{customerId}")]
        Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid customerId, [Header("Authorization")] string authorization);
    }
}
