using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.ResumeEvents.CQ
{
    public class GetResumeEventQuery : IRequest<ResumeEventViewModel>
    {
        public int Id { get; set; }
    }
}