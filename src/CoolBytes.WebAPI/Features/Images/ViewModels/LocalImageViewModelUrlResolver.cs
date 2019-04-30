using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI.Features.Images.ViewModels
{
    [Inject(typeof(IImageViewModelUrlResolver), ServiceLifetime.Scoped, "production")]
    public class LocalImageViewModelUrlResolver : IImageViewModelUrlResolver
    {
        public string Create(Image image)
        {
            if (image == null)
                return null;

            return image.UriPath;
        }
    }
}