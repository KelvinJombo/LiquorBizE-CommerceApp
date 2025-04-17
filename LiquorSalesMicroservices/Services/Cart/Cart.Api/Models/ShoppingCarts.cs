namespace Cart.Api.Models
{
    public class ShoppingCarts
    {
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(x => x.SellingPrice * x.Quantity);

        public ShoppingCarts(string userName)
        {
            UserName = userName;
        }


        //Required for Mapping
        public ShoppingCarts()
        {
            
        }
    }
}
