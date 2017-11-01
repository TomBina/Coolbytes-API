using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Data;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class AddAuthorCommandValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorCommandValidator(AppDbContext context)
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
            RuleFor(a => a.Experiences).SetCollectionValidator(new ExperienceDtoValidator(context));
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
