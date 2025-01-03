namespace LiquorSales.Web.Models.ShoppingCart
{
    public class ShoppingCartModel
    {
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItemModel> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(x => x.SellingPrice * x.Quantity);

    }

    public class ShoppingCartItemModel 
    {
        public int Quantity { get; set; } = default;
        public string Size { get; set; } = default!;
        public string Category { get; set; } = default!;
        public decimal SellingPrice { get; set; } = default!;
        public Guid ProductId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
    }


    public record GetCartResponse(ShoppingCartModel Cart);
    public record StoreCartRequest(ShoppingCartModel Cart);
    public record StoreCartResponse(string UserName);
    public record DeleteCartResponse(bool IsSucces);

}
