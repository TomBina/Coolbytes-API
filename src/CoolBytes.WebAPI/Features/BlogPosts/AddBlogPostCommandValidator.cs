using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Data;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class AddBlogPostCommandValidator : AbstractValidator<AddBlogPostCommand>
    {
        public AddBlogPostCommandValidator(AppDbContext appDbContext)
        {
            RuleFor(b => b.Subject).NotEmpty().MaximumLength(100);
            RuleFor(b => b.ContentIntro).NotEmpty().MaximumLength(100);
            RuleFor(b => b.Content).NotEmpty().MaximumLength(4000);
            RuleFor(b => b.AuthorId).CustomAsync(async (authorId, context, cancellationToken) =>
            {
                var author = await appDbContext.Authors.FindAsync(keyValues: new object[] { authorId }, cancellationToken: cancellationToken);
                if (author == null)
                    context.AddFailure(nameof(authorId), "Not found");
            });
        }
    }
}
