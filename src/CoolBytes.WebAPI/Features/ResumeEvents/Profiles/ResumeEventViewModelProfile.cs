using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Profiles
{
    public class ResumeEventViewModelProfile : Profile
    {
        public ResumeEventViewModelProfile()
        {
            CreateMap<ResumeEvent, ResumeEventViewModel>();
        }
    }
}
