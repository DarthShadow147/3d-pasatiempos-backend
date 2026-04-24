using _3d_pasatiempos_backend.Application.Dtos.CustomerDto;
using FluentValidation;

namespace _3d_pasatiempos_backend.Application.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Must have name");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Must have phone");
        }
    }
}
