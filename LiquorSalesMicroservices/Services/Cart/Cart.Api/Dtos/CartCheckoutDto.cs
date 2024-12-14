namespace Cart.Api.Dtos
{
    public class CartCheckoutDto
    {
        public string UserName { get; set; } = default!;
        public string CustomerId { get; set; } = default!;
        public decimal TotalPrice { get; set; } = default!;

        // Delivery and Company Address
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        //public string PhoneNumber { get; set; } = default!;
        public string AddressLine { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Town { get; set; } = default!;

        // Payment
        public string CardName { get; set; } = default!;
        public string CardNumber { get; set; } = default!;
        public string ExpiryDate { get; set; } = default!;
        public string CVV { get; set; } = default!;
        public int PaymentMethod { get; set; } = default!;
        public string PaymentRefNumber { get; set; } = default!;
    }
}
