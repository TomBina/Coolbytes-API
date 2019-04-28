using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Authors.ViewModels;

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
