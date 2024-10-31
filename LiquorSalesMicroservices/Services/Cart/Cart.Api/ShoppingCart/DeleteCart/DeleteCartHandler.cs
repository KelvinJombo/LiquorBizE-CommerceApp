using FluentValidation.AspNetCore;

namespace Cart.Api.ShoppingCart.DeleteCart
{
    public record DeleteCartCommand(string UserName) : ICommand<DeleteCartResult>;

    public record DeleteCartResult(bool IsSuccess);
    public class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
    {
        public DeleteCartCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName Is Required");
        }
    }
    public class DeleteCartCommandHandler : ICommandHandler<DeleteCartCommand, DeleteCartResult>
    {
        public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
        {
            return new DeleteCartResult(true);
        }
    }
}
