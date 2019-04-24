using CoolBytes.Core.Abstractions;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
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
