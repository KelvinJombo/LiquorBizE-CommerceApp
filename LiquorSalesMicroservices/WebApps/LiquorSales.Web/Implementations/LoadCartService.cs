
using Microsoft.AspNetCore.Http;
using System.Net;

namespace LiquorSales.Web.Implementations
{
    public class LoadCartService(ICartServices cartServices, IHttpContextAccessor _httpContextAccessor) : ILoadCartServices
    {
        public async Task<ShoppingCartModel> LoadUserCart()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            ShoppingCartModel cart;

            try
            {
                var getCartResponse = await cartServices.GetCart(userName, $"Bearer {token}");
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
