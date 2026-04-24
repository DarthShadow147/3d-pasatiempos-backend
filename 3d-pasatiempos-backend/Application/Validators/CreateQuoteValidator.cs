using _3d_pasatiempos_backend.Application.Dtos.QuoteDto;
using FluentValidation;

namespace _3d_pasatiempos_backend.Application.Validators
{
    public class CreateQuoteValidator : AbstractValidator<CreateQuoteDto>
    {
        public CreateQuoteValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Must select a valid customer");

            RuleFor(x => x.Printer)
                .GreaterThan(0)
                .WithMessage("Must select a valid printer");

            RuleFor(x => x.Material)
                .GreaterThan(0)
                .WithMessage("Must select a valid material");

            RuleFor(x => x.ProfitPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("The profit percentage must be between 0 and 100");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Must add at least one item");

            RuleForEach(x => x.Items).SetValidator(new CreateQuoteItemRequestValidator());
        }
    }
}
