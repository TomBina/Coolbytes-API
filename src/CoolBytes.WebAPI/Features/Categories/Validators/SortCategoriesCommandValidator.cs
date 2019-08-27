using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using FluentValidation;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Common;

namespace CoolBytes.WebAPI.Features.Categories.Validators
{
    public class SortCategoriesCommandValidator : AbstractValidator<SortCategoriesCommand>
    {
        public SortCategoriesCommandValidator(AppDbContext context)
        {
            RuleFor(r => r.NewSortOrder).CustomAsync(async (newSortOrder, validationContext, cancellationToken) =>
            {
                var sortValidator = new SortValidator<Category>(context);
                await sortValidator.Validate(newSortOrder);
            });
        }
    }
}
