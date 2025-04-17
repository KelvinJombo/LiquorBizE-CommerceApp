namespace Cart.Api.ShoppingCart.UpdateCart
{
    public record UpdateCartCommand(ShoppingCarts Cart) : ICommand<UpdateCartResult>;
    public record UpdateCartResult(string UserName);


    public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null.");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required.");
        }
    }


    public class UpdateCartHandler
    {
        public class UpdateCartCommandHandler(
        ICartRepository repository,
        DiscountProtoService.DiscountProtoServiceClient discountProto
    ) : ICommandHandler<UpdateCartCommand, UpdateCartResult>
        {
            public async Task<UpdateCartResult> Handle(UpdateCartCommand command, CancellationToken cancellationToken)
            {
                // Recalculate item prices using Discount.Grpc
                await ApplyLatestDiscounts(command.Cart, cancellationToken);

                // Update the cart in the database (upsert if needed)
                await repository.UpdateCart(command.Cart, cancellationToken);

                return new UpdateCartResult(command.Cart.UserName);
            }

            private async Task ApplyLatestDiscounts(ShoppingCarts cart, CancellationToken cancellationToken)
            {
                foreach (var item in cart.Items)
                {
                    var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest
                    {
                        ProductName = item.ProductName
                    }, cancellationToken: cancellationToken);

                    item.SellingPrice -= coupon.Amount;
                }
            }
        }
    }
}
