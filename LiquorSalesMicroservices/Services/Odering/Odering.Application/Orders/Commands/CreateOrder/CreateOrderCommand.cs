using BuildingBlocks.CQRS;
using FluentValidation;
using Odering.Application.Dtos;
using System.Windows.Input;

namespace Odering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

    public record CreateOrderResult(Guid Id);


    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is Required");
            RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is Required");
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderItem should not be Empty");
        }
    }
}
