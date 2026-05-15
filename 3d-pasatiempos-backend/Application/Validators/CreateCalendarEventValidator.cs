using _3d_pasatiempos_backend.Application.Dtos.CalendarEventsDto;
using FluentValidation;

namespace _3d_pasatiempos_backend.Application.Validators
{
    public class CreateCalendarEventValidator : AbstractValidator<CreateEventDto>
    {
        public CreateCalendarEventValidator()
        {
            RuleFor(x => x.EventName)
                .NotEmpty()
                .WithMessage("Must have a event name");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("It must have a start date");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("It must have an end date");
        }
    }
}
