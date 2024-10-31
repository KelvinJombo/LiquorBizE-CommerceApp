namespace Cart.Api.Data
{
    public interface ICartRepository
    {
        Task<ShoppingCarts> GetCart(string userName, CancellationToken cancellationToken = default);
        Task<ShoppingCarts> StoreCart(ShoppingCarts cart, CancellationToken cancellationToken = default);
        Task<bool> DeleteCart(string userName, CancellationToken cancellationToken = default);
    }
}
