using CoolBytes.Core.Attributes;
using CoolBytes.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CoolBytes.WebAPI.Features.Images.ViewModels
{
    public interface IImageViewModelFactory
    {
        ImageViewModel Create(Image image);
    }

    [Inject(typeof(IImageViewModelFactory), ServiceLifetime.Scoped, "production")]
    public class LocalImageViewModelFactory : IImageViewModelFactory
    {
        public ImageViewModel Create(Image image)
        {
            if (image == null)
                return new ImageViewModel();

            return new ImageViewModel() { Id = image.Id, UriPath = image.UriPath };
        }
    }
}