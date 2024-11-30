namespace Odering.Infrastructure.Data.Extensions
{
    public class InitialData
    {
        public static IEnumerable<Customer> Customers =>

            new List<Customer>
            {
                Customer.Create(CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")), "Kelly", "kelly@yahoo.cum", "08076543267"),
                Customer.Create(CustomerId.Of(new Guid("58c39479-ec65-4be2-86e7-033c546291aa")), "Chommy", "umeh@yahoo.cum", "08076543268"),
            };


        public static IEnumerable<Product> Product =>
            new List<Product>
            {
                Domain.Models.Product.Create(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff64")), "Gulder Beer", 5000m),
                Domain.Models.Product.Create(ProductId.Of(new Guid("5334c996-8457-4cf7-815c-ed2b77c4ff63")), "Star Beer", 3000m),
                Domain.Models.Product.Create(ProductId.Of(new Guid("5334c996-8357-4cf8-815c-ed2b77c4ff62")), "Heineken Beer", 8000m),
                Domain.Models.Product.Create(ProductId.Of(new Guid("5304c996-8457-4cf8-815c-ed2b77c4ff60")), "Hero Beer", 6000m)
            };



        public static IEnumerable<Order> OrdersWithItems
        {
            get
            {
                var address1 = Address.Of("Kelly", "Jombo", "kelly@yahoo.com", "No 12 Ngene Amawbia", "Awka", "Nigeria", "38050");
                var address2 = Address.Of("Chommy", "Changa", "umeh@gmail.com", "No 13 Uga Nnewi Str", "Wichitech Avenue", "Nigeria", "08050");

                var payment1 = Payment.Of("Kelly", "65656543322456", "12/23", "234", 1, "765335689111");
                var payment2 = Payment.Of("Chommy", "9393938383746", "11/30", "334", 2, "988664342133");

                var order1 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")),
                    OrderName.Of("Consignment 1"),
                    deliveryAddress: address1,
                    companyAddress: address1,
                    payment1);
                order1.Add(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff64")), 3, 5000);
                order1.Add(ProductId.Of(new Guid("5334c996-8457-4cf7-815c-ed2b77c4ff63")), 2, 4000);

                var order2 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("58c39479-ec65-4be2-86e7-033c546291aa")),
                    OrderName.Of("Order 2"),
                    deliveryAddress: address2,
                    companyAddress: address2,
                    payment2);
                order2.Add(ProductId.Of(new Guid("5334c996-8357-4cf8-815c-ed2b77c4ff62")), 5, 8000);
                order2.Add(ProductId.Of(new Guid("5304c996-8457-4cf8-815c-ed2b77c4ff60")), 7, 6000);

                return new List<Order> { order1, order2 };
            }
        }

    }
}
