
using System.Net;

namespace LiquorSales.Web.Implementations
{
    public class LoadCartService(ICartServices cartServices, IHttpContextAccessor _httpContextAccessor) : ILoadCartServices
    {
        public async Task<ShoppingCartModel> LoadUserCart()
        {
            // Get the currently authenticated user's username
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            ShoppingCartModel cart;

            try
            {
                var getCartResponse = await cartServices.GetCart(userName);
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
