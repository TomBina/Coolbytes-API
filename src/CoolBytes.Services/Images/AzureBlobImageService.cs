using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using CoolBytes.Services.BlobStorage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoolBytes.Services.Images
{
    [Inject(typeof(IImageService), ServiceLifetime.Scoped, "development", "production-azure")]
    public class AzureBlobImageService : IImageService
    {
        private readonly ImageFactory _imageFactory;
        private readonly IBlobClient _client;

        public AzureBlobImageService(ImageFactory imageFactory, IBlobClient client)
        {
            _imageFactory = imageFactory;
            _client = client;
        }

        public async Task Delete(Image image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            await _client.Delete(image.FileName);
        }

        public Task<Image> Save(Stream stream, string currentFileName, string contentType)
            => _imageFactory.Create(stream, currentFileName, contentType);
    }
}