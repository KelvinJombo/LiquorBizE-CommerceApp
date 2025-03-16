namespace LiquorSales.Web.Implementations
{
    public interface ILoadCartServices
    {
        Task<ShoppingCartModel> LoadUserCart();
    }
}
