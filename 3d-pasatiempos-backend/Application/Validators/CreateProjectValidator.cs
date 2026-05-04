using _3d_pasatiempos_backend.Application.Dtos.ProjectDto;
using FluentValidation;

namespace _3d_pasatiempos_backend.Application.Validators
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("A customer must be linked");

            RuleFor(x => x.ProjectName)
                .NotEmpty()
                .WithMessage("Must have name");
        }
    }
}
