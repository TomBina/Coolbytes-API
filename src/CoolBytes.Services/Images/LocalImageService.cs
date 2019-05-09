using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;

namespace CoolBytes.Services.Images
{
    [Inject(typeof(IImageService), ServiceLifetime.Scoped, "production-onprem")]
    public class LocalImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly ImageFactory _imageFactory;

        public LocalImageService(IConfiguration configuration, ImageFactory imageFactory)
        {
            _configuration = configuration;
            _imageFactory = imageFactory;
        }

        public Task Delete(Image image)
        {
            File.Delete($"{_configuration["ImagesUploadPath"]}{image.Path}");

            return Task.CompletedTask;
        }

        public async Task<Image> Save(Stream stream, string currentFileName, string contentType) 
            => await _imageFactory.Create(stream, currentFileName, contentType);
    }
}