namespace CatalogueApi.Products.CreateProduct
{
    public record CreateProductCommand(string Name, string CompanyName, List<string> Category, string Description, int StockingQuantity, string ImageFile, decimal CostPrice, decimal SellingPrice, DateOnly ExpiryDate)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is Required").Length(2, 50).WithMessage("Name characters must fall between 2 and 50");
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage("CompanyName is Required").Length(2, 100).WithMessage("Name characters must fall between 2 and 100");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is Required");
            RuleFor(x => x.StockingQuantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is Required");
            RuleFor(x => x.CostPrice).GreaterThan(0).WithMessage("Price is must be greater than 0");
            RuleFor(x => x.SellingPrice).GreaterThan(x => x.CostPrice).WithMessage("SellingPrice is must be greater than CostPrice");
            RuleFor(x => x.ExpiryDate).NotNull().WithMessage("ExpiryDate is required").GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("ExpiryDate must be greater than today's date");

        }
    } 
    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create Product Entity from Command Object
            var product = new Product
            {
                Name = command.Name,
                CompanyName = command.CompanyName,
                Category = command.Category,
                Description = command.Description,
                StockingQuantity = command.StockingQuantity,
                ImageFile = command.ImageFile,
                CostPrice = command.CostPrice,
                SellingPrice = command.SellingPrice,
                ExpiryDate = command.ExpiryDate
            };

            // Save to Database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // Return the CreateProductResult
            return new CreateProductResult(product.Id);

        }
    }
}
