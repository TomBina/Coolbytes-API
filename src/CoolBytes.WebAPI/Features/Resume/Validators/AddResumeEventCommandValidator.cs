using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Features.Resume.CQ;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.Resume.Validators
{
    public class AddResumeEventCommandValidator : AbstractValidator<AddResumeEventCommand>
    {
        public AddResumeEventCommandValidator()
        {
            RuleFor(c => c.DateRange).NotNull();
            RuleFor(c => c.DateRange.EndDate).GreaterThan(cv => cv.DateRange.StartDate);
            RuleFor(c => c.Name).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Message).NotEmpty().MaximumLength(1000);
        }
    }
}
