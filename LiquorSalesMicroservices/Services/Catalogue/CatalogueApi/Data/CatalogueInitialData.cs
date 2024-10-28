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
            session.Store<Product>(GetPreConfiguredProduct());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreConfiguredProduct() => new List<Product>() {


                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Name = "Star Lager Beer",
                    Description = "Oldest NBL most famous Beer Brand",
                    Quantity = 500,
                    ImageFile = "Star Image Link",
                    Price = 5000,
                    Category = new List<string> { "NBL", "Green" }


                },


                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff62"),
                    Name = "Gulder Lager Beer",
                    Description = "Another Oldest NBL most famous Beer Brand",
                    Quantity = 450,
                    ImageFile = "Gulder Image Link",
                    Price = 5650,
                    Category = new List<string> { "NBL", "Brown" }

                },

                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff63"),
                    Name = "Start Radler Beer",
                    Description = "NBL famous Light Beer Brand",
                    Quantity = 600,
                    ImageFile = "Star Radler Image Link",
                    Price = 3000,
                    Category = new List<string> { "NBL", "White"}


                },


                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff64"),
                    Name = "Heineken Lager Beer",
                    Description = "Foreign NBL most famous Beer Brand",
                    Quantity = 1000,
                    ImageFile = "Heineken Image Link",
                    Price = 8000,
                    Category = new List<string> { "NBL", "Green" }


                }


        };
    }
}
