using AutoMapper;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Images
{
    public class ImageViewModelProfile : Profile
    {
        public ImageViewModelProfile()
        {
            CreateMap<Image, ImageViewModel>().ForMember(v => v.UriPath, exp => exp.MapFrom(p => p.UriPath));
        }
    }
}
