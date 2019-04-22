using CoolBytes.Core.Domain;
using CoolBytes.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.Services.ImageFactories
{
    [Inject(typeof(ImageFactory), ServiceLifetime.Scoped, "production-azure")]
    public class AzureBlobImageFactory : ImageFactory
    {
        private readonly IHostingEnvironment _environment;
        private readonly string _connectionString;

        public AzureBlobImageFactory(IConfiguration configuration, IHostingEnvironment environment, IImageFactoryValidator validator) : base(validator)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("BlobStorage").Replace("{KEY}", configuration["storagekey"]);
        }

        public override async Task<Image> Create(Stream stream, string currentFileName, string contentType)
        {
            Validate(stream, currentFileName, contentType);

            var blobRef = CreateBlobReference(currentFileName);
            await blobRef.UploadFromStreamAsync(stream);
            
            return new Image(blobRef.Name, blobRef.Container.Name, blobRef.Name, blobRef.Properties.Length, contentType);
        }

        private CloudBlockBlob CreateBlobReference(string currentFileName)
        {
            var container = _environment.EnvironmentName;
            var client = CloudStorageAccount.Parse(_connectionString).CreateCloudBlobClient();
            var containerRef = client.GetContainerReference(container);
            var fileName = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(currentFileName)}";
            var blobRef = containerRef.GetBlockBlobReference(fileName);

            return blobRef;
        }
    }
}
