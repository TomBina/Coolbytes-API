using CoolBytes.WebAPI.Features.Resume.DTO;
using CoolBytes.WebAPI.Features.Resume.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Resume.CQ
{
    public class UpdateResumeEventCommand : IRequest<ResumeEventViewModel>
    {
        public int Id { get; set; }
        public DateRangeDto DateRange { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
    }
}