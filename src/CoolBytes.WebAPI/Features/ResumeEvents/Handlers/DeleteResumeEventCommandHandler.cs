using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class DeleteResumeEventCommandHandler : IAsyncRequestHandler<DeleteResumeEventCommand>
    {
        private readonly AppDbContext _context;

        public DeleteResumeEventCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteResumeEventCommand message)
        {
            var resumeEvent = await _context.ResumeEvents.FirstOrDefaultAsync(r => r.Id == message.Id);
            _context.ResumeEvents.Remove(resumeEvent);

            await _context.SaveChangesAsync();
        }
    }
}