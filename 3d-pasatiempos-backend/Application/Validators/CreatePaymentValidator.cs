using _3d_pasatiempos_backend.Application.Dtos.PaymentDto;
using FluentValidation;

namespace _3d_pasatiempos_backend.Application.Validators
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");

            RuleFor(x => x.Method)
                .NotEmpty()
                .WithMessage("The payment method must be indicated");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("The type of payment must be indicated");
        }
    }
}
