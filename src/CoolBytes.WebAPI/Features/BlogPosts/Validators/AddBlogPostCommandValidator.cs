using System.Linq;
using CoolBytes.Core.Interfaces;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.BlogPosts.Validators
{
    public class AddBlogPostCommandValidator : AbstractValidator<AddBlogPostCommand>
    {
        public AddBlogPostCommandValidator(IUserService userService, IAuthorValidator authorValidator)
        {
            RuleFor(b => b.Subject).NotEmpty().MaximumLength(100);
            RuleFor(b => b.ContentIntro).NotEmpty().MaximumLength(120);
            RuleFor(b => b.Content).NotEmpty().MaximumLength(8000);
            RuleFor(b => b.Tags).Custom((tags, context) =>
            {
                if (tags == null)
                    return;

                var invalidTags = tags.Where(tag => string.IsNullOrWhiteSpace(tag));

                foreach (var invalidTag in invalidTags)
                {
                    context.AddFailure(nameof(tags), "Empty tag not allowed.");
                }
            });
            RuleFor(b => b).CustomAsync(async (command, context, cancellationToken) =>
            {
                if (!await authorValidator.Exists(userService))
                    context.AddFailure("No author registered.");
            });
        }
    }
}