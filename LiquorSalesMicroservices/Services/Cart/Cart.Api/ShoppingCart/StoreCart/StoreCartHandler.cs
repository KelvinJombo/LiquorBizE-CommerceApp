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
    public class StoreCartCommandHandler(ICartRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto) : ICommandHandler<StoreCartCommand, StoreCartResult>
    {
        public async Task<StoreCartResult> Handle(StoreCartCommand command, CancellationToken cancellationToken)
        {
            //Communicate with Discount.Grpc and Calculate latest price of products in the ShoppingCart
            await DeductDiscount(command.Cart, cancellationToken);
             
            await repository.StoreCart(command.Cart, cancellationToken);

            return new StoreCartResult(command.Cart.UserName);

        }


        private async Task DeductDiscount(ShoppingCarts cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }

    }
}
