using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Images.ViewModels;

namespace CoolBytes.WebAPI.Features.Images.Profiles.Resolvers
{
    public class ImageViewModelResolver : IValueResolver<Image, ImageViewModel, string>
    {
        private readonly IImageViewModelFactory _factory;

        public ImageViewModelResolver(IImageViewModelFactory factory)
        {
            _factory = factory;
        }

        public string Resolve(Image source, ImageViewModel destination, string destMember, ResolutionContext context) 
            => _factory.Create(source).UriPath;
    }
}