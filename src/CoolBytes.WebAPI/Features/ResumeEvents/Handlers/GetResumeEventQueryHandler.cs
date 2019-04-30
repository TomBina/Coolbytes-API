using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class GetResumeEventQueryHandler : IRequestHandler<GetResumeEventQuery, ResumeEventViewModel>
    {
        private readonly HandlerContext<ResumeEventViewModel> _context;
        private readonly AppDbContext _dbContext;

        public GetResumeEventQueryHandler(HandlerContext<ResumeEventViewModel> context)
        {
            _context = context;
            _dbContext = context.DbContext;
        }

        public async Task<ResumeEventViewModel> Handle(GetResumeEventQuery message, CancellationToken cancellationToken)
        {
            var resumeEvent = await _dbContext.ResumeEvents.FindAsync(message.Id);

            return _context.Map(resumeEvent);
        }
    }
}