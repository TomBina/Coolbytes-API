using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Validators
{
    public class GetResumeEventsQueryValidator : AbstractValidator<GetResumeEventsQuery>
    {
        public GetResumeEventsQueryValidator(AppDbContext context)
        {
            RuleFor(q => q.AuthorId).CustomAsync(async (id, validationContext, cancellationToken) =>
            {
                if (!await context.Authors.AnyAsync(a => a.Id == id))
                    validationContext.AddFailure(validationContext.PropertyName, "No Author found");
            });
        }
    }
}
