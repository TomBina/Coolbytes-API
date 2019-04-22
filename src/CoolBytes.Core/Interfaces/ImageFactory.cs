using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Interfaces
{
    public abstract class ImageFactory
    {
        private readonly IImageFactoryValidator _validator;

        protected ImageFactory(IImageFactoryValidator validator)
        {
            _validator = validator;
        }

        protected void Validate(Stream stream, string currentFileName, string contentType)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (currentFileName == null) throw new ArgumentNullException(nameof(currentFileName));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));

            if (!_validator.Validate(stream, contentType)) throw new ArgumentException("Image is not valid");
        }

        public abstract Task<Image> Create(Stream stream, string currentFileName, string contentType);
    }
}