using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class UpdateResumeEventHandler : IRequestHandler<UpdateResumeEventCommand, ResumeEventViewModel>
    {
        private readonly HandlerContext<ResumeEventViewModel> _context;
        private readonly AppDbContext _dbContext;

        public UpdateResumeEventHandler(HandlerContext<ResumeEventViewModel> context)
        {
            _context = context;
            _dbContext = context.DbContext;
        }

        public async Task<ResumeEventViewModel> Handle(UpdateResumeEventCommand message, CancellationToken cancellationToken)
        {
            var resumeEvent = GetResumeEvent(message);

            Update(message, resumeEvent);

            await Save();

            return ViewModel(resumeEvent);
        }

        private async Task Save() 
            => await _dbContext.SaveChangesAsync();

        private ResumeEvent GetResumeEvent(UpdateResumeEventCommand message) 
            => _dbContext.ResumeEvents.FirstOrDefault(r => r.Id == message.Id);

        private ResumeEventViewModel ViewModel(ResumeEvent resumeEvent) 
            => _context.Map(resumeEvent);

        private static void Update(UpdateResumeEventCommand message, ResumeEvent resumeEvent)
        {
            resumeEvent.DateRange.Update(message.DateRange.StartDate, message.DateRange.EndDate);
            resumeEvent.Update(message.Name, message.Message);
        }
    }
}