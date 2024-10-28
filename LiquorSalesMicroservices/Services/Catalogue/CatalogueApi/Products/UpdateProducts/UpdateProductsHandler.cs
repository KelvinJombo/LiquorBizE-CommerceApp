
namespace CatalogueApi.Products.UpdateProducts
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, int Quantity, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(command => command.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name field cannot be empty")
                .Length(2, 150).WithMessage("Name characters must fall between 2 and 150");
            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class UpdateProductsCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.Quantity = command.Quantity;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
