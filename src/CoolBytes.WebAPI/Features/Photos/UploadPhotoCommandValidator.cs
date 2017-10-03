using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class UploadPhotoCommandValidator : AbstractValidator<UploadPhotosCommand>
    {
        public UploadPhotoCommandValidator()
        {
            RuleFor(p => p.Files).NotNull();
        }
    }
}
