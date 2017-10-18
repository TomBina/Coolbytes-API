using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Resume.CQ;
using CoolBytes.WebAPI.Features.Resume.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Resume.Handlers
{
    public class UpdateResumeEventHandler : IAsyncRequestHandler<UpdateResumeEventCommand, ResumeEventViewModel>
    {
        private readonly AppDbContext _context;

        public UpdateResumeEventHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResumeEventViewModel> Handle(UpdateResumeEventCommand message)
        {
            var resumeEvent = GetResumeEvent(message);

            Update(message, resumeEvent);

            await Save();

            return ViewModel(resumeEvent);
        }

        private async Task Save() 
            => await _context.SaveChangesAsync();

        private ResumeEvent GetResumeEvent(UpdateResumeEventCommand message) 
            => _context.ResumeEvents.FirstOrDefault(r => r.Id == message.Id);

        private static ResumeEventViewModel ViewModel(ResumeEvent resumeEvent) 
            => Mapper.Map<ResumeEventViewModel>(resumeEvent);

        private static void Update(UpdateResumeEventCommand message, ResumeEvent resumeEvent)
        {
            var dateRange = new DateRange(message.DateRange.StartDate, message.DateRange.EndDate);

            resumeEvent.Update(dateRange, message.Name, message.Message);
        }
    }
}