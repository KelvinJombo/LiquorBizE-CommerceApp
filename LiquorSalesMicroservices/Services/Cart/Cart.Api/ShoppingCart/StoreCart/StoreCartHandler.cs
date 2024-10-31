namespace Cart.Api.ShoppingCart.StoreCart
{
    public record StoreCartCommand(ShoppingCarts Cart) : ICommand<StoreCartResult>;
    public record StoreCartResult(string UserName);

    public class StoreCartCammandValidator : AbstractValidator<StoreCartCommand>
    {
        public StoreCartCammandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart Cannot Be Null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName Is Required");
        }
    }
    public class StoreCartCommandHandler : ICommandHandler<StoreCartCommand, StoreCartResult>
    {
        public async Task<StoreCartResult> Handle(StoreCartCommand command, CancellationToken cancellationToken)
        {
             ShoppingCarts cart = command.Cart;

            // Store Cart in Database using Marten Upsert & Session.Store

            return new StoreCartResult("swn");

        }
    }
}
