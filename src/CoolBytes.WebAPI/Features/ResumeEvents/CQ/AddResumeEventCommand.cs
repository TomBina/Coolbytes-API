using CoolBytes.WebAPI.Features.ResumeEvents.DTO;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.ResumeEvents.CQ
{
    public class AddResumeEventCommand : IRequest<ResumeEventViewModel>
    {
        public DateRangeDto DateRange { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
    }
}