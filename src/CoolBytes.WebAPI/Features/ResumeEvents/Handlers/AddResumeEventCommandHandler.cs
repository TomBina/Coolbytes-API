using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Interfaces;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class AddResumeEventCommandHandler : IRequestHandler<AddResumeEventCommand, ResumeEventViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IAuthorService _authorService;

        public AddResumeEventCommandHandler(AppDbContext context, IAuthorService authorService)
        {
            _context = context;
            _authorService = authorService;
        }

        public async Task<ResumeEventViewModel> Handle(AddResumeEventCommand message, CancellationToken cancellationToken)
        {
            var resumeEvent = await CreateResumeEvent(message);

            await Save(resumeEvent);

            return ViewModel(resumeEvent);
        }

        private async Task<ResumeEvent> CreateResumeEvent(AddResumeEventCommand message)
        {
            var dateRange = new DateRange(message.DateRange.StartDate, message.DateRange.EndDate);
            var author = await _authorService.GetAuthor();

            var resumeEvent = new ResumeEvent(author, dateRange, message.Name, message.Message);
            return resumeEvent;
        }

        private async Task Save(ResumeEvent resumeEvent)
        {
            _context.ResumeEvents.Add(resumeEvent);
            await _context.SaveChangesAsync();
        }

        private static ResumeEventViewModel ViewModel(ResumeEvent resumeEvent)
        {
            return Mapper.Map<ResumeEventViewModel>(resumeEvent);
        }
    }
}