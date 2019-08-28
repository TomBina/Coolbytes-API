using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using FluentValidation;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
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
                var result = await sortValidator.Validate(newSortOrder);

                if (result is ErrorResult error)
                {
                    validationContext.AddFailure(error.ErrorMessage);
                }
            });
        }
    }
}
