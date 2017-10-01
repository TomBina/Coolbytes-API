using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using FluentValidation;
using System.Linq;
using CoolBytes.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class UpdateBlogPostCommandValidator : AbstractValidator<UpdateBlogPostCommand>
    {
        public UpdateBlogPostCommandValidator(AppDbContext appDbContext, IUserService userService, IAuthorValidator authorValidator)
        {
            RuleFor(b => b.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
            {
                var user = await userService.GetUser();
                var blogPost = await appDbContext.BlogPosts.FirstOrDefaultAsync(b => b.Author.User.Id == user.Id);
                if (blogPost == null)
                    context.AddFailure(nameof(id), "Updating blogpost can only be done by the author.");
            });
            RuleFor(b => b.Subject).NotEmpty().MaximumLength(100);
            RuleFor(b => b.ContentIntro).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Content).NotEmpty().MaximumLength(4000);
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
        }
    }
}