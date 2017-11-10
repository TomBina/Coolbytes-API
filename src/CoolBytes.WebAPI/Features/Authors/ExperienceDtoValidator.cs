using CoolBytes.Core.Models;
using CoolBytes.Data;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class ExperienceDtoValidator : AbstractValidator<ExperienceDto>
    {
        public ExperienceDtoValidator(AppDbContext context)
        {
            RuleFor(e => e.Id).CustomAsync(async (id, validationContext, cancellationToken) =>
            {
                if (id == null)
                    return;

                var experience = await context.FindAsync<Experience>(id);
                if (experience == null)
                {
                    validationContext.AddFailure("Id", "No experience found");
                }
            });
            RuleFor(e => e.Name).NotEmpty().MaximumLength(50);
            RuleFor(e => e.Color).NotEmpty().MinimumLength(6).MaximumLength(6);
            RuleFor(e => e.ImageId).NotEmpty().CustomAsync(async (id, validationContext, cancellationToken) =>
            {
                var image = await context.Images.FindAsync(id);
                if (image == null)
                {
                    validationContext.AddFailure("ImageId", "No image found");
                }
            });
        }
    }
}