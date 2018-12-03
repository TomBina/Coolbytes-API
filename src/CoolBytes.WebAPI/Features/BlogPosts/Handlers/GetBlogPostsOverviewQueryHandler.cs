using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using CoolBytes.WebAPI.Services.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class GetBlogPostsOverviewQueryHandler : IRequestHandler<GetBlogPostsOverviewQuery, BlogPostsOverviewViewModel>
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public GetBlogPostsOverviewQueryHandler(AppDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
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

        private Task<List<CategoryViewModel>> QueryBlogPosts() =>
            _context.BlogPosts
                .AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .OrderByDescending(b => b.Id)
                .GroupBy(b => b.CategoryId)
                .Select(b => new CategoryViewModel()
                {
                    CategoryId = b.Key,
                    Category = b.FirstOrDefault().Category.Name,
                    BlogPosts = Mapper.Map<List<BlogPostSummaryViewModel>>(b.AsEnumerable())
                })
                .ToListAsync();
    }
}
