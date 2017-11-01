using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Validators
{
    public class AddResumeEventCommandValidator : AbstractValidator<AddResumeEventCommand>
    {
        public AddResumeEventCommandValidator()
        {
            RuleFor(c => c.DateRange).NotNull();
            RuleFor(c => c.DateRange.StartDate).NotNull().When(c => c.DateRange != null);
            RuleFor(c => c.DateRange.EndDate).NotNull().GreaterThanOrEqualTo(cv => cv.DateRange.StartDate).When(c => c.DateRange != null);
            RuleFor(c => c.Name).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Message).NotEmpty().MaximumLength(1000);
        }
    }
}
