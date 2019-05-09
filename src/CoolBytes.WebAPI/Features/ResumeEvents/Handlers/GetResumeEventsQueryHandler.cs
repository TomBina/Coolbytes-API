using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class GetResumeEventsQueryHandler : IRequestHandler<GetResumeEventsQuery, IEnumerable<ResumeEventViewModel>>
    {
        private readonly HandlerContext<IEnumerable<ResumeEventViewModel>> _context;
        private readonly AppDbContext _dbContext;

        public GetResumeEventsQueryHandler(HandlerContext<IEnumerable<ResumeEventViewModel>> context)
        {
            _context = context;
            _dbContext = context.DbContext;
        }

        public async Task<IEnumerable<ResumeEventViewModel>> Handle(GetResumeEventsQuery message, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == message.AuthorId);
            var resumeEvents = await _dbContext.ResumeEvents.Where(r => r.AuthorId == author.Id)
                                                          .OrderByDescending(r => r.DateRange.StartDate)
                                                          .ToListAsync();

            return _context.Map(resumeEvents);
        }
    }
}