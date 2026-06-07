using FluentValidation;

namespace OrderManagement.Application.Orders.CreateOrder
{
    public class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0);
        }
    }
}