using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Authors.ViewModels;

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
