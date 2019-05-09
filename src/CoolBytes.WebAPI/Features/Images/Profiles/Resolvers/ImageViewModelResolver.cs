using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Images.ViewModels;

namespace CoolBytes.WebAPI.Features.Images.Profiles.Resolvers
{
    public class ImageViewModelResolver : IValueResolver<Image, ImageViewModel, string>
    {
        private readonly IImageViewModelUrlResolver _urlResolver;

        public ImageViewModelResolver(IImageViewModelUrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public string Resolve(Image source, ImageViewModel destination, string destMember, ResolutionContext context) 
            => _urlResolver.Create(source);
    }
}