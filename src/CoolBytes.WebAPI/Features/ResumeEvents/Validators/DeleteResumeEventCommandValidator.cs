using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Validators
{
    public class DeleteResumeEventCommandValidator : AbstractValidator<DeleteResumeEventCommand>
    {
        public DeleteResumeEventCommandValidator(AppDbContext context, IAuthorService authorService)
        {
            RuleFor(d => d.Id).CustomAsync(async (id, validationContext, cancellationToken) =>
            {
                var author = await authorService.GetAuthor();

                if (!await context.ResumeEvents.AnyAsync(r => r.Id == id && r.AuthorId == author.Id))
                    validationContext.AddFailure(validationContext.PropertyName, "No ResumeEvent found");
            });
        }
    }
}
