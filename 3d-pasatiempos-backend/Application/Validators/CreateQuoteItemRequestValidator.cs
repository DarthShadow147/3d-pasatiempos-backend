using _3d_pasatiempos_backend.Application.Dtos.QuoteDto;
using FluentValidation;

namespace _3d_pasatiempos_backend.Application.Validators
{
    public class CreateQuoteItemRequestValidator : AbstractValidator<CreateQuoteItemRequest>
    {
        public CreateQuoteItemRequestValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("The product name is required");

            RuleFor(x => x.Grams)
                .GreaterThan(0)
                .WithMessage("The grams used must be greater than 0");

            RuleFor(x => x.Hours)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The hours cannot be negative");

            RuleFor(x => x.Minutes)
                .InclusiveBetween(0, 59)
                .WithMessage("The minutes must be between 0 and 59");
        }
    }
}
