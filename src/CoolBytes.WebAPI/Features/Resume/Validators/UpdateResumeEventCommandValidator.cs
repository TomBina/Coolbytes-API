using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Resume.CQ;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Resume.Validators
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
            RuleFor(c => c.DateRange.EndDate).GreaterThan(cv => cv.DateRange.StartDate);
            RuleFor(c => c.Name).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Message).NotEmpty().MaximumLength(1000);
        }
    }
}
