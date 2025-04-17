namespace LiquorSales.Web.Implementations
{
    public interface ILoadCartServices
    {
        Task<UpdateCartResponse> AddToCart(Guid productId, string productName, decimal sellingPrice, int quantity, string size, List<string> categories);
        Task<ShoppingCartModel> LoadUserCart();
    }
}
