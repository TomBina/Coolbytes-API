using AutoMapper;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Authors.Profiles
{
    public class ExperienceViewModelProfile : Profile
    {
        public ExperienceViewModelProfile()
        {
            CreateMap<Experience, ExperienceViewModel>();
        }
    }
}
