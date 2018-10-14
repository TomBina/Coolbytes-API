using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Resume
{
    public class GetResumeQueryHandler : IRequestHandler<GetResumeQuery, ResumeViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IAuthorSearchService _authorSearchService;

        public GetResumeQueryHandler(AppDbContext context, IAuthorSearchService authorSearchService)
        {
            _context = context;
            _authorSearchService = authorSearchService;
        }

        public async Task<ResumeViewModel> Handle(GetResumeQuery message, CancellationToken cancellationToken)
        {
            var resume = await CreateResume(message);

            return Mapper.Map<ResumeViewModel>(resume);
        }

        private async Task<Core.Models.Resume> CreateResume(GetResumeQuery message)
        {
            var author = await _authorSearchService.GetAuthorWithProfile(message.AuthorId);

            var resumeEvents = await _context.ResumeEvents.Where(r => r.AuthorId == message.AuthorId).ToListAsync();

            var resume = new Core.Models.Resume(author, resumeEvents);
            return resume;
        }
    }
}