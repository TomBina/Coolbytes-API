using CoolBytes.WebAPI.Features.Resume.DTO;
using CoolBytes.WebAPI.Features.Resume.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Resume.CQ
{
    public class AddResumeEventCommand : IRequest<ResumeEventViewModel>
    {
        public DateRangeDto DateRange { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
    }
}