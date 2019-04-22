using System;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace CoolBytes.Services.ImageFactories
{
    public class AzureBlobImageFactory : ImageFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly IImageFactoryValidator _validator;

        public AzureBlobImageFactory(IConfiguration configuration, IHostingEnvironment environment, IImageFactoryValidator validator) : base(validator)
        {
            _configuration = configuration;
            _environment = environment;
            _validator = validator;
        }
        public override async Task<Image> Create(Stream stream, string currentFileName, string contentType)
        {
            Validate(stream, currentFileName, contentType);

            var container = _environment.EnvironmentName;
            var client = CloudStorageAccount.Parse("").CreateCloudBlobClient();
            var containerRef = client.GetContainerReference(container);
            var fileName = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(currentFileName)}";
            var fileLength = stream.Length;
            var blobRef = containerRef.GetBlockBlobReference(fileName);

            await blobRef.UploadFromStreamAsync(stream);

            return new Image(fileName, "", "", fileLength, contentType);
        }
    }
}
