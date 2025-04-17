using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Cart.Api.Data
{
    public class CachedCartRepository(ICartRepository cartRepository, IDistributedCache cache) : ICartRepository
    {
        

        public async Task<ShoppingCarts> GetCart(string userName, CancellationToken cancellationToken = default)
        {
            var cachedCart = await cache.GetStringAsync(userName, cancellationToken);
            if(!string.IsNullOrEmpty(cachedCart)) 
              return JsonSerializer.Deserialize<ShoppingCarts>(cachedCart)!;
             

            var cart = await cartRepository.GetCart(userName, cancellationToken);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(cart), cancellationToken);
            return cart;

        }

        public async Task<ShoppingCarts> StoreCart(ShoppingCarts cart, CancellationToken cancellationToken = default)
        {
            await cartRepository.StoreCart(cart, cancellationToken);
            await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);
            return cart;
        }


        public async Task<ShoppingCarts> UpdateCart(ShoppingCarts cart, CancellationToken cancellationToken = default)
        {
            await cartRepository.UpdateCart(cart, cancellationToken);
            await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);
            return cart;
        }



        public async Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default)
        {
              await cartRepository.DeleteCart(userName, cancellationToken);

            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }
    }
}
