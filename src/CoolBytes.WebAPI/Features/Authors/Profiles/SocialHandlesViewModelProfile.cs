using AutoMapper;
using CoolBytes.Core.Models;

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
