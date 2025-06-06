﻿namespace Cart.Api.Data
{
    public class CartRepository(IDocumentSession session) : ICartRepository
    { 
        public async Task<ShoppingCarts> GetCart(string userName, CancellationToken cancellationToken = default)
        {
             var cart = await session.LoadAsync<ShoppingCarts>(userName, cancellationToken);

            return cart is null ? throw new CartNotFoundException(userName) : cart;
        }

        public async Task<ShoppingCarts> StoreCart(ShoppingCarts cart, CancellationToken cancellationToken = default)
        {
            session.Store(cart);
            await session.SaveChangesAsync(cancellationToken);
            return cart;
        }

        public async Task<ShoppingCarts> UpdateCart(ShoppingCarts cart, CancellationToken cancellationToken = default)
        {
            session.Update(cart);
            await session.SaveChangesAsync(cancellationToken);
            return cart;
        }


        public async Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default)
        {
            var cart = await session.LoadAsync<ShoppingCarts>(userName, cancellationToken);
            if (cart == null)
            {
                throw new CartNotFoundException(userName);
            }

            session.Delete(cart);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}
