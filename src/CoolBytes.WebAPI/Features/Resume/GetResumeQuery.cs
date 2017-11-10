using MediatR;

namespace CoolBytes.WebAPI.Features.Resume
{
    public class GetResumeQuery : IRequest<ResumeViewModel>
    {
        public int AuthorId { get; set; }
    }
}