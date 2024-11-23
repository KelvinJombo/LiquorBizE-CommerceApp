namespace CatalogueApi.Data
{
    public class CatalogueInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if (await session.Query<Product>().AnyAsync())
                return;

            //Marten UPSERT will cater for existing records
            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreConfiguredProducts() => new List<Product>() {


                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Name = "Star Lager Beer",
                    CompanyName = "Nigerian Brewery",
                    Description = "Oldest NBL most famous Beer Brand",
                    StockingQuantity = 500,
                    ImageFile = "Star Image Link",
                    CostPrice = 5000,
                    SellingPrice = 5000,
                    Category = new List<string> { "NBL", "Green" },
                    ExpiryDate = new DateOnly(2025, 12, 31)

                },


                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff62"),
                    Name = "Gulder Lager Beer",
                    CompanyName = "Nigerian Brewery",
                    Description = "Another Oldest NBL most famous Beer Brand",
                    StockingQuantity = 450,
                    ImageFile = "Gulder Image Link",
                    CostPrice = 5650,
                    SellingPrice = 5650,
                    Category = new List<string> { "NBL", "Brown" },
                    ExpiryDate = new DateOnly(2025, 12, 31)

                },

                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff63"),
                    Name = "Start Radler Beer",
                    CompanyName = "Nigerian Brewery",
                    Description = "NBL famous Light Beer Brand",
                    StockingQuantity = 600,
                    ImageFile = "Star Radler Image Link",
                    CostPrice = 3000,
                    SellingPrice = 3000,
                    Category = new List<string> { "NBL", "White"},
                    ExpiryDate = new DateOnly(2025, 12, 31)



                },


                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff64"),
                    Name = "Heineken Lager Beer",
                    CompanyName = "Nigerian Brewery",
                    Description = "Foreign NBL most famous Beer Brand",
                    StockingQuantity = 1000,
                    ImageFile = "Heineken Image Link",
                    CostPrice = 8000,
                    SellingPrice = 8000,
                    Category = new List<string> { "NBL", "Green" },
                    ExpiryDate = new DateOnly(2025, 12, 31)


                }


        };
    }
}
