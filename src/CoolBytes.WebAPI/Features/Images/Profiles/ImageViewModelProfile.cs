using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Images.Profiles.Resolvers;
using CoolBytes.WebAPI.Features.Images.ViewModels;

namespace CoolBytes.WebAPI.Features.Images.Profiles
{
    public class ImageViewModelProfile : Profile
    {
        public ImageViewModelProfile()
        {
            CreateMap<Image, ImageViewModel>().ForMember(i => i.UriPath, exp => exp.MapFrom<ImageViewModelResolver>());
        }
    }
}
