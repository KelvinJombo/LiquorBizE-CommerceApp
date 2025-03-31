namespace CatalogueApi.Models
{
    public class Product
    {
        private int _stockingQuantity;

        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public string Size { get; set; } = default!;
        public List<string> Category { get; set; } = new();

        public int StockingQuantity
        {
            get => _stockingQuantity;
            set
            {
                _stockingQuantity = value;
                TotalQuantity += value;  
            }
        }

        public int TotalQuantity { get; private set; }
        public string Description { get; set; } = default!;
        public string ImagePath { get; set; } = default!;
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public DateTime? StockingDate { get; set; } = DateTime.Now;
        public DateOnly? ExpiryDate { get; set; }
    }

}
