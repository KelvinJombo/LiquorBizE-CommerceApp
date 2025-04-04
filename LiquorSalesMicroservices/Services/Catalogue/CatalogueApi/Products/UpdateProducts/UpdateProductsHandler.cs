﻿
namespace CatalogueApi.Products.UpdateProducts
{
    public record UpdateProductCommand(Guid Id, string Name, string CompanyName, string Size, List<string> Category, string Description, string ImagePath, decimal SellingPrice, int StockingQuantity, DateOnly? ExpiryDate)
        : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name field cannot be empty")
                .Length(2, 50).WithMessage("Name characters must fall between 2 and 50");
            RuleFor(command => command.CompanyName).NotEmpty().WithMessage("Company Name field cannot be empty")
                .Length(2, 150).WithMessage("CompanyName characters must fall between 2 and 150");
            RuleFor(command => command.Size).NotEmpty().WithMessage("Product Size is Required");
            RuleFor(command => command.ExpiryDate).NotNull().WithMessage("ExpiryDate is required").GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("ExpiryDate must be greater than today's date");

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
            product.CompanyName = command.CompanyName;
            product.Size = command.Size;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImagePath = command.ImagePath;
            product.SellingPrice = command.SellingPrice;
            product.StockingQuantity = command.StockingQuantity;
            product.ExpiryDate = command.ExpiryDate;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
