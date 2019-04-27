using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Services.BlobStorage;
using CoolBytes.Services.Images.Factories;

namespace CoolBytes.Services.Images
{
    public class AzureBlobImageService : IImageService
    {
        private readonly AzureBlobImageFactory _imageFactory;
        private readonly AzureBlobClient _client;

        public AzureBlobImageService(AzureBlobImageFactory imageFactory, AzureBlobClient client)
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