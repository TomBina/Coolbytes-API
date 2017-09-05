using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;

namespace CoolBytes.Core
{
    public class PhotoFactory : IPhotoFactory
    {
        private readonly PhotoFactoryOptions _options;
        private readonly IPhotoValidator _validator;

        public PhotoFactory(PhotoFactoryOptions options, IPhotoValidator validator)
        {
            _options = options;
            _validator = validator;
        }

        public async Task<Photo> Create(Stream stream, string currentFileName, string contentType)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (currentFileName == null) throw new ArgumentNullException(nameof(currentFileName));

            if (!_validator.Validate(stream, contentType)) throw new ArgumentException("Image is not valid");

            var extension = Path.GetExtension(currentFileName) ?? throw new ArgumentNullException(nameof(currentFileName));
            var newFileName = Path.Combine(_options.UploadPath, _options.GetFileName(extension));
            var length = stream.Length;
            
            using (var fileStream = File.Create(newFileName))
            {
                await stream.CopyToAsync(fileStream);
            }

            return new Photo(newFileName, _options.UploadPath, length, contentType);
        }
    }
}

