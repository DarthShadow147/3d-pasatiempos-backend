using _3d_pasatiempos_backend.Application.Dtos.OrderDto;
using FluentValidation;

namespace _3d_pasatiempos_backend.Application.Validators
{
    public class CreateProductionValidator : AbstractValidator<CreateProductionDto>
    {
        public CreateProductionValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("Must select a valid order");

            RuleFor(x => x.Hours)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The hours cannot be negative");

            RuleFor(x => x.Minutes)
                .InclusiveBetween(0, 59)
                .WithMessage("The minutes must be between 0 and 59");
        }
    }
}
