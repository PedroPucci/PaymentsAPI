using FluentValidation;
using PaymentsAPI.Domain.Entitities;

namespace PaymentsAPI.Application.Validators
{
    public class PaymentRequestValidator
        : AbstractValidator<PaymentEntity>
    {
        public PaymentRequestValidator()
        {
            RuleFor(payment => payment.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(payment => payment.GameId)
                .GreaterThan(0)
                .WithMessage("GameId must be greater than zero.");

            RuleFor(payment => payment.Amount)
                .GreaterThan(0)
                .WithMessage("Payment amount must be greater than zero.");

            RuleFor(payment => payment.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment method is required.")
                .MaximumLength(50)
                .WithMessage(
                    "Payment method must have a maximum of 50 characters.");
        }
    }
}