using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoolBytes.Core.Factories
{
    public class ImageFactory : IImageFactory
    {
        private readonly IImageFactoryOptions _options;
        private readonly IImageFactoryValidator _validator;

        public ImageFactory(IImageFactoryOptions options, IImageFactoryValidator validator)
        {
            _options = options;
            _validator = validator;
        }

        public async Task<Image> Create(Stream stream, string currentFileName, string contentType)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (currentFileName == null) throw new ArgumentNullException(nameof(currentFileName));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));

            if (!_validator.Validate(stream, contentType)) throw new ArgumentException("Image is not valid");

            var extension = Path.GetExtension(currentFileName);
            var fileName = _options.FileName(extension);
            var directory = _options.Directory(_options.UploadPath, fileName);
            var path = Path.Combine(directory, fileName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var length = stream.Length;

            using (var fileStream = File.Create(path))
            {
                await stream.CopyToAsync(fileStream);
            }

            var uriPath = _options.UriPath(fileName);
            var pathWithoutUploadPath = path.Replace(_options.UploadPath, string.Empty);
            return new Image(fileName, pathWithoutUploadPath, uriPath, length, contentType);
        }
    }
}

