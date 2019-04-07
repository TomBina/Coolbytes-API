using AutoMapper;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Authors.Profiles
{
    public class SocialHandlesViewModelProfile : Profile
    {
        public SocialHandlesViewModelProfile()
        {
            CreateMap<SocialHandles, SocialHandlesViewModel>();
        }
    }
}
