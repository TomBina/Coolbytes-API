using System;
using CoolBytes.Core.Abstractions;
using CoolBytes.Data;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator(AppDbContext context, IUserService userService)
        {
            RuleFor(a => a.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.LastName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.About).NotEmpty().MaximumLength(500);
            RuleFor(a => a.ImageId).CustomAsync(async (id, validationContext, cancellationToken) =>
            {
                if (id != null)
                {
                    var image = await context.Images.FindAsync(id);
                    if (image == null)
                    {
                        validationContext.AddFailure("ImageId", "No image found");
                    }
                }
            });
            RuleFor(a => a.ResumeUri).Custom(ValidateUri);
            RuleFor(a => a.LinkedIn).Custom(ValidateUri);
            RuleFor(a => a.GitHub).Custom(ValidateUri);
            RuleForEach(a => a.Experiences).SetValidator(new ExperienceDtoValidator(context));
            RuleFor(a => a).CustomAsync(async (command, validationContext, token) =>
            {
                var user = await userService.TryGetCurrentUserAsync();

                if (!user)
                    validationContext.AddFailure("No user found");

                if (!await context.Authors.AnyAsync(a => a.UserId == user.Payload.Id))
                    validationContext.AddFailure("No author found!");
            });
        }

        private void ValidateUri(string uri, CustomContext context)
        {
            if (uri == null)
                return;

            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                context.AddFailure(context.PropertyName, "Valid URI is required");
            }
        }
    }
}