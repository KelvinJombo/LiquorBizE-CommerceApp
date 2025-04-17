using System.Net;

namespace LiquorSales.Web.Implementations
{
    public class LoadCartService(ICartServices cartServices, IHttpContextAccessor _httpContextAccessor) : ILoadCartServices
    {
        public async Task<UpdateCartResponse> AddToCart(Guid productId, string productName, decimal sellingPrice, int quantity, string size, List<string> categories)
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            var cart = await LoadUserCart();  

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new ShoppingCartItemModel
                {
                    ProductId = productId,
                    ProductName = productName,
                    SellingPrice = sellingPrice,
                    Quantity = quantity,
                    Size = size,
                    Categories = categories
                });
            }

            var updateRequest = new UpdateCartRequest(cart);
            var response = await cartServices.UpdateCart(updateRequest, $"Bearer {token}");

            return response;
        }



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
