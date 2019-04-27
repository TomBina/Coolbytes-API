using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;

namespace CoolBytes.Services.ImageFactories
{
    [Inject(typeof(ImageFactory), ServiceLifetime.Scoped, "production")]
    public class LocalImageFactory : ImageFactory
    {
        private readonly IImageFactoryOptions _options;

        public LocalImageFactory(IImageFactoryOptions options, IImageFactoryValidator validator) : base(validator)
        {
            _options = options;
        }

        public override async Task<Image> Create(Stream stream, string currentFileName, string contentType)
        {
            Validate(stream, currentFileName, contentType);

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

