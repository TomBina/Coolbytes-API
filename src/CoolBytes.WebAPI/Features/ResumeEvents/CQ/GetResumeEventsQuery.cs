using System.Collections.Generic;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.ResumeEvents.CQ
{
    public class GetResumeEventsQuery : IRequest<IEnumerable<ResumeEventViewModel>>
    {
        public int AuthorId { get; set; }
    }
}