using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class DeleteResumeEventCommandHandler : AsyncRequestHandler<DeleteResumeEventCommand>
    {
        private readonly AppDbContext _context;

        public DeleteResumeEventCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task Handle(DeleteResumeEventCommand message, CancellationToken cancellationToken)
        {
            var resumeEvent = await _context.ResumeEvents.FirstOrDefaultAsync(r => r.Id == message.Id);
            _context.ResumeEvents.Remove(resumeEvent);

            await _context.SaveChangesAsync();
        }
    }
}