using System;
using System.Threading.Tasks;
using CoolBytes.Core.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.Services.BlobStorage
{
    [Inject(typeof(IBlobClient), ServiceLifetime.Scoped, "development", "azure-production")]
    public class BlobClient : IBlobClient
    {
        private readonly Lazy<CloudBlobClient> _client;
        private readonly string _containerName;

        public BlobClient(IConfiguration configuration, IHostingEnvironment environment)
        {
            _containerName = environment.EnvironmentName.ToLower();
            var connectionString = configuration.GetConnectionString("BlobStorage").Replace("{KEY}", configuration["storagekey"]);
            _client = new Lazy<CloudBlobClient>(() => CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient());
        }

        public CloudBlockBlob Get(string name) 
            => GetBlobReference(name);

        public CloudBlockBlob Create(string name) 
            => GetBlobReference(name);

        private CloudBlockBlob GetBlobReference(string name)
        {
            var client = _client.Value;
            var container = client.GetContainerReference(_containerName);

            return container.GetBlockBlobReference(name);
        }

        public async Task Delete(string name)
        {
            var client = _client.Value;
            var container = client.GetContainerReference(_containerName);

            await container.GetBlockBlobReference(name).DeleteAsync();
        }
    }
}