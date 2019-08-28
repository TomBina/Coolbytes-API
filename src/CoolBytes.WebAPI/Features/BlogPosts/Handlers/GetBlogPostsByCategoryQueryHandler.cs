using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.Services.Caching;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class GetBlogPostsByCategoryQueryHandler : IRequestHandler<GetBlogPostsByCategoryQuery, IEnumerable<BlogPostSummaryViewModel>>
    {
        private readonly HandlerContext<IEnumerable<BlogPostSummaryViewModel>> _context;
        private readonly AppDbContext _dbContext;
        private readonly ICacheService _cacheService;

        public GetBlogPostsByCategoryQueryHandler(HandlerContext<IEnumerable<BlogPostSummaryViewModel>> context)
        {
            _context = context;
            _dbContext = context.DbContext;
            _cacheService = context.Cache;
        }

        public async Task<IEnumerable<BlogPostSummaryViewModel>> Handle(GetBlogPostsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var blogs = await _cacheService.GetOrAddAsync(() => BlogPostsFactoryAsync(request.CategoryId), request.CategoryId);

            return _context.Map(blogs);
        }

        private async Task<IEnumerable<BlogPost>> BlogPostsFactoryAsync(int categoryId) 
            => await _dbContext.BlogPosts.AsNoTracking()
                                       .Include(b => b.Author)
                                       .Include(b => b.Author.AuthorProfile)
                                       .Include(b => b.Image)
                                       .Include(b => b.Category)
                                       .Where(b => b.CategoryId == categoryId)
                                       .OrderBy(b => b.SortOrder)
                                       .ToListAsync();
    }
}