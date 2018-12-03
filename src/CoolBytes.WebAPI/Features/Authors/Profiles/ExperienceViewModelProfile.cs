using AutoMapper;
using CoolBytes.Core.Models;

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
