using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI.Features.Images.ViewModels
{
    [Inject(typeof(IImageViewModelFactory), ServiceLifetime.Scoped, "development", "production-azure")]
    public class AzureBlobImageViewModelFactory : IImageViewModelFactory
    {
        private string _environment;

        public AzureBlobImageViewModelFactory(IHostingEnvironment environment)
        {
            _environment = environment.EnvironmentName.ToLower();
        }

        public ImageViewModel Create(Image image)
        {
            if (image == null)
                return new ImageViewModel();

            var currentUri = image.UriPath;
            if (currentUri.Contains("/images"))
            {
                currentUri = currentUri.Split('/')[2];
            }

            return new ImageViewModel() { Id = image.Id, UriPath = currentUri };
        }
    }
}
