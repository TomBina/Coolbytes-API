using System.Linq;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Categories.Validators
{
    public class SortCategoriesCommandValidator : AbstractValidator<SortCategoriesCommand>
    {
        public SortCategoriesCommandValidator(AppDbContext context)
        {
            RuleFor(r => r.NewSortOrder).CustomAsync(async (newSortOrder, validationContext, cancellationToken) =>
            {
                var newSortOrderCount = newSortOrder.Count();
                var allUnique = newSortOrderCount == newSortOrder.Distinct().Count();

                if (!allUnique)
                {
                    validationContext.AddFailure(Resources.Features.Categories.Validators.SortCategoriesCommandValidator.IdsMustBeUniqueError);
                    return;
                }

                var categoryIds = (await context.Categories.Select(c => c.Id).ToListAsync()).ToHashSet();

                if (newSortOrder.Count != categoryIds.Count)
                {
                    validationContext.AddFailure(Resources.Features.Categories.Validators.SortCategoriesCommandValidator.SortOrderLengthError);
                    return;
                }

                var allExist = newSortOrder.All(i => categoryIds.Contains(i));

                if (!allExist)
                {
                    validationContext.AddFailure(Resources.Features.Categories.Validators.SortCategoriesCommandValidator.AllIdsMustExistError);
                }
            });
        }
    }
}
