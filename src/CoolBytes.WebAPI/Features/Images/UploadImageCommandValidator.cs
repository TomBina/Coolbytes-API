using FluentValidation;

namespace CoolBytes.WebAPI.Features.Images
{
    public class UploadImageCommandValidator : AbstractValidator<UploadImagesCommand>
    {
        public UploadImageCommandValidator()
        {
            RuleFor(p => p.Files).NotNull();
        }
    }
}
