using MediatR;

namespace CoolBytes.WebAPI.Features.ResumeEvents.CQ
{
    public class DeleteResumeEventCommand : IRequest
    {
        public int Id { get; set; }
    }
}