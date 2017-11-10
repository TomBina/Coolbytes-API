using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Validators
{
    public class UpdateResumeEventCommandValidator : AbstractValidator<UpdateResumeEventCommand>
    {
        public UpdateResumeEventCommandValidator(AppDbContext context)
        {
            RuleFor(c => c.Id).CustomAsync(async (id, validationContext, cancellationToken) =>
            {
                if (!await context.ResumeEvents.AnyAsync(r => r.Id == id))
                    validationContext.AddFailure(validationContext.PropertyName, "No ResumeEvent found");
            });
            RuleFor(c => c.DateRange).NotNull();
            RuleFor(c => c.DateRange.EndDate).GreaterThanOrEqualTo(cv => cv.DateRange.StartDate);
            RuleFor(c => c.Name).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Message).NotEmpty().MaximumLength(1000);
        }
    }
}
