using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.BlogPosts.Validators
{
    public class DeleteBlogPostCommandValidator : AbstractValidator<DeleteBlogPostCommand>
    {
        public DeleteBlogPostCommandValidator(AppDbContext appDbContext)
        {
            RuleFor(b => b.Id).CustomAsync(async (id, context, cancellationToken) =>
            {
                var blogPost = await appDbContext.BlogPosts.FindAsync(keyValues: new object[] {id},
                    cancellationToken: cancellationToken);
                if (blogPost == null)
                    context.AddFailure(nameof(id), "Deleting blogpost can only be done by the author.");
            });
        }
    }
}