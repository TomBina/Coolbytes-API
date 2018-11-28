using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Services.Caching;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, BlogPostsViewModel>
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public GetBlogPostsQueryHandler(AppDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<BlogPostsViewModel> Handle(GetBlogPostsQuery message, CancellationToken cancellationToken)
        {
            var viewModel = await _cacheService.GetOrAddAsync(() => CreateViewModelAsync());

            return viewModel;
        }

        private async Task<BlogPostsViewModel> CreateViewModelAsync()
        {
            var categories = await QueryBlogPosts();
            var viewmodel = new BlogPostsViewModel { Categories = categories };

            return viewmodel;
        }

        private Task<List<BlogPost>> QueryBlogPostsWithTag(string tag) =>
            _context.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .Where(b => b.Tags.Any(t => t.Name == tag))
                .OrderByDescending(b => b.Id)
                .ToListAsync();

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
