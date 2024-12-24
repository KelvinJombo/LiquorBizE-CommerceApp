namespace LiquorSales.Web.Services
{
    public interface ICartServices
    {
        [Get("/cart-service/cart/{userName}")]
        Task<GetCartResponse> GetCart(string userName);

        [Post("/cart-service/cart")]
        Task<StoreCartResponse> StoreCart(StoreCartRequest request);

        [Delete("/cart-service/cart/{userName}")]
        Task<DeleteCartResponse> DeleteCart(string userName);

        [Post("/cart-service/cart/checkout")]
        Task<CheckoutCartResponse> CheckoutCart(CheckoutCartRequest request);

    }
}
