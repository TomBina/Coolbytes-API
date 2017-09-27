using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class AddBlogPostCommandValidator : AbstractValidator<AddBlogPostCommand>
    {
        public AddBlogPostCommandValidator(AppDbContext appDbContext, IUserService userService, IAuthorValidator authorValidator)
        {
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
            RuleFor(b => b).CustomAsync(async (command, context, cancellationToken) =>
            {
                if (!await authorValidator.Exists(userService))
                    context.AddFailure("No author registered.");
            });
        }
    }
}