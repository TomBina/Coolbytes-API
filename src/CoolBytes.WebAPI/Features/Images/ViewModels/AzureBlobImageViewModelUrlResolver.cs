using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI.Features.Images.ViewModels
{
    [Inject(typeof(IImageViewModelUrlResolver), ServiceLifetime.Scoped, "development", "production-azure")]
    public class AzureBlobImageViewModelUrlResolver : IImageViewModelUrlResolver
    {
        public string Create(Image image)
        {
            if (image == null)
                return null;

            var currentUri = image.UriPath;
            if (currentUri.Contains("/images"))
            {
                currentUri = currentUri.Split('/')[2];
            }

            return currentUri;
        }
    }
}
