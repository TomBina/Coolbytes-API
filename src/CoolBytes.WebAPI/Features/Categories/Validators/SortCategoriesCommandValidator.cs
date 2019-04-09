using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var newSortOrderCount = newSortOrder.Count;
                var currentSortOrderCount = await context.Categories.CountAsync();

                if (newSortOrderCount != currentSortOrderCount)
                {
                    validationContext.AddFailure(Resources.Features.Categories.Validators.SortCategoriesCommandValidator.SortOrderError);
                }
            });
        }
    }
}
