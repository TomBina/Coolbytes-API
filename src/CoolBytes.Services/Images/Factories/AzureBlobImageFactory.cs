using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using CoolBytes.Services.BlobStorage;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.Services.Images.Factories
{
    [Inject(typeof(ImageFactory), ServiceLifetime.Scoped, "development", "production-azure")]
    public class AzureBlobImageFactory : ImageFactory
    {
        private readonly IBlobClient _client;

        public AzureBlobImageFactory(IBlobClient client, IImageFactoryValidator validator) : base(validator)
        {
            _client = client;
        }

        public override async Task<Image> Create(Stream stream, string currentFileName, string contentType)
        {
            Validate(stream, currentFileName, contentType);

            var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(currentFileName)}";
            var blob = _client.Create(fileName);
            blob.Properties.ContentType = contentType;

            await blob.UploadFromStreamAsync(stream);
            
            return new Image(blob.Name, blob.Container.Name, blob.Name, blob.Properties.Length, contentType);
        }
    }
}