using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.ResumeEvents.DTO;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Profiles
{
    public class DateRangeDtoProfile : Profile
    {
        public DateRangeDtoProfile()
        {
            CreateMap<DateRange, DateRangeDto>();
        }
    }
}
