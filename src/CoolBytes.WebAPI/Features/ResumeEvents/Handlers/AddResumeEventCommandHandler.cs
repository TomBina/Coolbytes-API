using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class AddResumeEventCommandHandler : IRequestHandler<AddResumeEventCommand, ResumeEventViewModel>
    {
        private readonly AppDbContext _dbContext;
        private readonly HandlerContext<ResumeEventViewModel> _context;
        private readonly IAuthorService _authorService;

        public AddResumeEventCommandHandler(HandlerContext<ResumeEventViewModel> context, IAuthorService authorService)
        {
            _dbContext = context.DbContext;
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
            _dbContext.ResumeEvents.Add(resumeEvent);

            await _dbContext.SaveChangesAsync();
        }

        private ResumeEventViewModel ViewModel(ResumeEvent resumeEvent)
        {
            return _context.Map(resumeEvent);
        }
    }
}