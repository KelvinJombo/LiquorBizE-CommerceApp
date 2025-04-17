using System.ComponentModel.DataAnnotations;

namespace LiquorSales.Web.Models.ShoppingCart
{
    public class ShoppingCartModel
    {
        public string UserName { get; set; } = default!;
        public List<ShoppingCartItemModel> Items { get; set; } = new();
        public decimal TotalPrice => Items?.Sum(i => i.SellingPrice * i.Quantity) ?? 0;
    }

    public class ShoppingCartItemModel 
    {
        public int Quantity { get; set; } = default;
        public string Size { get; set; } = default!;
        public List<string> Categories { get; set; } = default!;
        public decimal SellingPrice { get; set; } = default!;
        public Guid ProductId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
    }


    public class UpdateRequest
    {
        [Required]
        public string? ProductName { get; set; }
        public decimal SellingPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }



    public record GetCartResponse(ShoppingCartModel Cart);
    public record StoreCartRequest(ShoppingCartModel Cart);
    public record UpdateCartRequest(ShoppingCartModel Cart);
    public record UpdateCartResponse(string UserName);
    public record StoreCartResponse(string UserName);
    public record DeleteCartResponse(bool IsSucces);

}
