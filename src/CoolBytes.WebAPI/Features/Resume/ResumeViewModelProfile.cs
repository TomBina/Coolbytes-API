using AutoMapper;

namespace CoolBytes.WebAPI.Features.Resume
{
    public class ResumeViewModelProfile : Profile
    {
        public ResumeViewModelProfile()
        {
            CreateMap<Core.Domain.Resume, ResumeViewModel>();
        }
    }
}
