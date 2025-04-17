namespace Cart.Api.Models
{
    public class ShoppingCartItem
    {
        public int Quantity { get; set; } = default!;
        public string Size { get; set; } = default!;
        public string Category { get; set; } = default!;
        public decimal SellingPrice { get; set; } = default!;
        public Guid ProductId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
    }
}
