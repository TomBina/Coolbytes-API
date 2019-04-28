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
    public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, IEnumerable<BlogPostSummaryViewModel>>
    {
        private readonly HandlerContext<IEnumerable<BlogPostSummaryViewModel>> _context;
        private readonly AppDbContext _dbContext;
        private readonly ICacheService _cache;

        public GetBlogPostsQueryHandler(HandlerContext<IEnumerable<BlogPostSummaryViewModel>> context)
        {
            _context = context;
            _dbContext = context.DbContext;
            _cache = context.Cache;
        }

        public async Task<IEnumerable<BlogPostSummaryViewModel>> Handle(GetBlogPostsQuery message, CancellationToken cancellationToken)
        {
            IEnumerable<BlogPostSummaryViewModel> viewModel;

            if (message.Tag == null)
            {
                viewModel = await _cache.GetOrAddAsync(() => ViewModelAsync(null));
            }

            else
            {
                viewModel = await ViewModelAsync(message.Tag);
            }

            return viewModel;
        }

        private async Task<IEnumerable<BlogPostSummaryViewModel>> ViewModelAsync(string tag)
        {
            IEnumerable<BlogPost> blogPosts;

            if (tag == null)
            {
                blogPosts = await QueryBlogPosts();
            }
            else
            {
                blogPosts = await QueryBlogPostsWithTag(tag);
            }

            return _context.Map(blogPosts);
        }

        private Task<List<BlogPost>> QueryBlogPostsWithTag(string tag) =>
            _dbContext.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .Include(b => b.Category)
                .Where(b => b.Tags.Any(t => t.Name == tag))
                .OrderByDescending(b => b.Id)
                .ToListAsync();

        private Task<List<BlogPost>> QueryBlogPosts() =>
            _dbContext.BlogPosts
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Image)
                .Include(b => b.Category)
                .OrderByDescending(b => b.Id)
                .ToListAsync();
    }
}