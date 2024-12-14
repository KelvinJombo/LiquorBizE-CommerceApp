
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Cart.Api.ShoppingCart.CheckoutCart
{
    public record CheckoutCartCommand(CartCheckoutDto CartCheckoutDto) : ICommand<CheckoutCartResult>;

    public record CheckoutCartResult(bool IsSuccess);

    public class CheckoutCartCommandValidator : AbstractValidator<CheckoutCartCommand>
    {
        public CheckoutCartCommandValidator()
        {
            RuleFor(x => x.CartCheckoutDto).NotNull().WithMessage("CartCheckoutDto Cannot be Null");
            RuleFor(x => x.CartCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
    
    public class CheckoutCartCommandHandler(ICartRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutCartCommand, CheckoutCartResult>
    {
        public async Task<CheckoutCartResult> Handle(CheckoutCartCommand command, CancellationToken cancellationToken)
        {
            var cart = await repository.GetCart(command.CartCheckoutDto.UserName, cancellationToken);
            if (cart == null)
            {
                return new CheckoutCartResult(false);
            }

            var eventMessage = command.CartCheckoutDto.Adapt<CartCheckoutEvent>();
            eventMessage.TotalPrice = cart.TotalPrice;

            await publishEndpoint.Publish(eventMessage, cancellationToken);

            await repository.DeleteCart(command.CartCheckoutDto.UserName, cancellationToken);

            return new CheckoutCartResult(true);
        }
    }
}
