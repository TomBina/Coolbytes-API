using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Images.ViewModels
{
    public interface IImageViewModelFactory
    {
        ImageViewModel Create(Image image);
    }
}