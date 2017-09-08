using CoolBytes.Data;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class UpdateBlogPostCommandValidator : AbstractValidator<UpdateBlogPostCommand>
    {
        public UpdateBlogPostCommandValidator(AppDbContext appDbContext)
        {
            RuleFor(b => b.Id).NotEmpty().CustomAsync(async (id, context, cancellationToken) =>
            {
                var blogPost = await appDbContext.BlogPosts.FindAsync(keyValues: new object[] {id}, cancellationToken: cancellationToken);
                if (blogPost == null)
                    context.AddFailure(nameof(id), "BlogPost not found");
            });
            RuleFor(b => b.Subject).NotEmpty().MaximumLength(100);
            RuleFor(b => b.ContentIntro).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Content).NotEmpty().MaximumLength(4000);
            RuleFor(b => b.AuthorId).NotEmpty().CustomAsync(async (authorId, context, cancellationToken) =>
            {
                var author = await appDbContext.Authors.FindAsync(keyValues: new object[] { authorId }, cancellationToken: cancellationToken);
                if (author == null)
                    context.AddFailure(nameof(authorId), "Author not found");
            });
        }
    }
}