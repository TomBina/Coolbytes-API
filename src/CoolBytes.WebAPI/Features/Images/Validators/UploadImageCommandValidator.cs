using CoolBytes.WebAPI.Features.Images.CQ;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.Images.Validators
{
    public class UploadImageCommandValidator : AbstractValidator<UploadImagesCommand>
    {
        public UploadImageCommandValidator()
        {
            RuleFor(p => p.Files).NotNull();
        }
    }
}
