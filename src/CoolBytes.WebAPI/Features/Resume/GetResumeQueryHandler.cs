using CoolBytes.Data;
using CoolBytes.Services;
using CoolBytes.Services.Caching;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Resume
{
    public class GetResumeQueryHandler : IRequestHandler<GetResumeQuery, ResumeViewModel>
    {
        private readonly AppDbContext _dbContext;
        private readonly HandlerContext<ResumeViewModel> _context;
        private readonly IAuthorSearchService _authorSearchService;
        private readonly ICacheService _cacheService;

        public GetResumeQueryHandler(HandlerContext<ResumeViewModel> context, IAuthorSearchService authorSearchService, ICacheService cacheService)
        {
            _dbContext = context.DbContext;
            _context = context;
            _authorSearchService = authorSearchService;
            _cacheService = cacheService;
        }

        public async Task<ResumeViewModel> Handle(GetResumeQuery message, CancellationToken cancellationToken)
        {
            var viewModel = await _cacheService.GetOrAddAsync(() => CreateViewModelAsync(message), message.AuthorId);

            return viewModel;
        }

        public async Task<ResumeViewModel> CreateViewModelAsync(GetResumeQuery message)
        {
            var resume = await CreateResumeAsync(message.AuthorId);

            return _context.Map(resume);
        }

        private async Task<Core.Domain.Resume> CreateResumeAsync(int authorId)
        {
            var author = await _authorSearchService.GetAuthorWithProfile(authorId);
            var resumeEvents = await _dbContext.ResumeEvents.Where(r => r.AuthorId == authorId).ToListAsync();

            var resume = new Core.Domain.Resume(author, resumeEvents);
            return resume;
        }
    }
}