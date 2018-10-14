using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class GetResumeEventQueryHandler : IRequestHandler<GetResumeEventQuery, ResumeEventViewModel>
    {
        private readonly AppDbContext _context;

        public GetResumeEventQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResumeEventViewModel> Handle(GetResumeEventQuery message, CancellationToken cancellationToken)
        {
            var resumeEvent = await _context.ResumeEvents.FindAsync(message.Id);

            return Mapper.Map<ResumeEventViewModel>(resumeEvent);
        }
    }
}