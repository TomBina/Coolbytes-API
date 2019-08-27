using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Common;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.BlogPosts.Validators
{
    public class SortBlogsCommandValidator : AbstractValidator<SortBlogsCommand>
    {
        public SortBlogsCommandValidator(AppDbContext context)
        {
            RuleFor(r => r.NewSortOrder).CustomAsync(async (newSortOrder, validationContext, cancellationToken) =>
            {
                var instance = (SortBlogsCommand)validationContext.InstanceToValidate;

                var sortValidator = new SortValidator<BlogPost>(context);
                var result = await sortValidator.Validate(newSortOrder, b => b.CategoryId == instance.CategoryId);

                if (result is ErrorResult error)
                {
                    validationContext.AddFailure(error.ErrorMessage);
                }
            });
        }
    }
}
