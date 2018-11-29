using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Services;
using CoolBytes.WebAPI.Services.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Resume
{
    public class GetResumeQueryHandler : IRequestHandler<GetResumeQuery, ResumeViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IAuthorSearchService _authorSearchService;
        private readonly ICacheService _cacheService;

        public GetResumeQueryHandler(AppDbContext context, IAuthorSearchService authorSearchService, ICacheService cacheService)
        {
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

            return Mapper.Map<ResumeViewModel>(resume);
        }

        private async Task<Core.Models.Resume> CreateResumeAsync(int authorId)
        {
            var author = await _authorSearchService.GetAuthorWithProfile(authorId);
            var resumeEvents = await _context.ResumeEvents.Where(r => r.AuthorId == authorId).ToListAsync();

            var resume = new Core.Models.Resume(author, resumeEvents);
            return resume;
        }
    }
}