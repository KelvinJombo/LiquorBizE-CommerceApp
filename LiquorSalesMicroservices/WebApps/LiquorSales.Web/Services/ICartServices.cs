using System.Net;

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

        public async Task<ShoppingCartModel> LoadUserCart() 
        {
            //Get Cart, and If not existing already, Create new Cart with Default Logged In User Name

            var userName = "Kelly";
            ShoppingCartModel cart;

            try
            {
                var getCartResponse = await GetCart(userName);

                cart = getCartResponse.Cart;
            }
            catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
            {
                cart = new ShoppingCartModel
                {
                    UserName = userName,
                    Items = []
                };

            }
            return cart;

        }
    }
}
