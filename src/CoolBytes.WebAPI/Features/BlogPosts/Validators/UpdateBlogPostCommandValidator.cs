using System.Linq;
using CoolBytes.Core.Interfaces;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Validators
{
    public class UpdateBlogPostCommandValidator : AbstractValidator<UpdateBlogPostCommand>
    {
        public UpdateBlogPostCommandValidator(AppDbContext appDbContext, IUserService userService)
        {
            RuleFor(b => b.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
            {
                var user = await userService.TryGetCurrentUser();

                if (!user)
                {
                    context.AddFailure("No user found.");
                    return;
                }

                var blogPost = await appDbContext.BlogPosts.FirstOrDefaultAsync(b => b.Author.User.Id == user.Payload.Id);
                if (blogPost == null)
                    context.AddFailure(nameof(id), "Updating blogpost can only be done by the author.");
            });
            RuleFor(b => b.Subject).NotEmpty().MaximumLength(100);
            RuleFor(b => b.ContentIntro).NotEmpty().MaximumLength(120);
            RuleFor(b => b.Content).NotEmpty().MaximumLength(8000);
            RuleFor(b => b.Tags).Custom((tags, context) =>
            {
                if (tags == null)
                    return;

                var invalidTags = tags.Where(tag => string.IsNullOrWhiteSpace(tag));

                foreach (var _ in invalidTags)
                {
                    context.AddFailure(nameof(tags), "Empty tag not allowed.");
                }
            });
        }
    }
}