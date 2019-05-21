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
    public class GetBlogPostsOverviewQueryHandler : IRequestHandler<GetBlogPostsOverviewQuery, BlogPostsOverviewViewModel>
    {
        private readonly HandlerContext<BlogPostsOverviewViewModel> _context;
        private readonly AppDbContext _dbContext;
        private readonly ICacheService _cacheService;

        public GetBlogPostsOverviewQueryHandler(HandlerContext<BlogPostsOverviewViewModel> context)
        {
            _context = context;
            _dbContext = context.DbContext;
            _cacheService = context.Cache;
        }

        public async Task<BlogPostsOverviewViewModel> Handle(GetBlogPostsOverviewQuery message, CancellationToken cancellationToken)
        {
            var viewModel = await _cacheService.GetOrAddAsync(() => CreateViewModelAsync());

            return viewModel;
        }

        private async Task<BlogPostsOverviewViewModel> CreateViewModelAsync()
        {
            var categories = await QueryBlogPosts();
            var viewmodel = new BlogPostsOverviewViewModel { Categories = categories };

            return viewmodel;
        }

        private Task<List<CategoryBlogPostsViewModel>> QueryBlogPosts() =>
            _dbContext.BlogPosts
                .AsNoTracking()
                .Include(b => b.Content)
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .OrderByDescending(b => b.Id)
                .GroupBy(b => b.CategoryId)
                .Select(b => new CategoryBlogPostsViewModel()
                {
                    CategoryId = b.Key,
                    Category = b.First().Category.Name,
                    SortOrder = b.First().Category.SortOrder,
                    BlogPosts = _context.Mapper.Map<List<BlogPostSummaryViewModel>>(b.AsEnumerable())
                })
                .OrderBy(c => c.SortOrder)
                .ToListAsync();
    }
}
