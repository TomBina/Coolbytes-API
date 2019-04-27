using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Services.Images.Factories;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.Services.Images
{
    public class LocalImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly LocalImageFactory _imageFactory;

        public LocalImageService(IConfiguration configuration, LocalImageFactory imageFactory)
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