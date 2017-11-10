using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.ResumeEvents.CQ;
using CoolBytes.WebAPI.Features.ResumeEvents.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.ResumeEvents.Handlers
{
    public class GetResumeEventsQueryHandler : IAsyncRequestHandler<GetResumeEventsQuery, IEnumerable<ResumeEventViewModel>>
    {
        private readonly AppDbContext _context;

        public GetResumeEventsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResumeEventViewModel>> Handle(GetResumeEventsQuery message)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == message.AuthorId);
            var resumeEvents = await _context.ResumeEvents.Where(r => r.AuthorId == author.Id)
                                                          .OrderByDescending(r => r.DateRange.StartDate)
                                                          .ToListAsync();

            return Mapper.Map<IEnumerable<ResumeEventViewModel>>(resumeEvents);
        }
    }
}