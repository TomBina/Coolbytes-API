using CoolBytes.Data;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class DeleteBlogPostCommandValidator : AbstractValidator<DeleteBlogPostCommand>
    {
        private readonly AppDbContext _appDbContext;

        public DeleteBlogPostCommandValidator(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

            RuleFor(b => b.Id).CustomAsync(async (id, context, cancellationToken) =>
            {
                var blogPost = await appDbContext.BlogPosts.FindAsync(keyValues: new object[] {id},
                    cancellationToken: cancellationToken);
                if (blogPost == null)
                    context.AddFailure(nameof(id), "BlogPost not found");
            });
        }
    }
}