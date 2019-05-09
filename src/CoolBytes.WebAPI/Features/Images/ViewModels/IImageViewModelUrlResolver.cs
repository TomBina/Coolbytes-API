using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Images.ViewModels
{
    public interface IImageViewModelUrlResolver
    {
        string Create(Image image);
    }
}