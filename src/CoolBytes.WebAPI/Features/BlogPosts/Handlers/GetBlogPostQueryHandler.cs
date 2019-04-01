using CoolBytes.Core.Models;
using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Services.Caching;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class GetBlogPostQueryHandler : IRequestHandler<GetBlogPostQuery, Result<BlogPostViewModel>>
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public GetBlogPostQueryHandler(AppDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<Result<BlogPostViewModel>> Handle(GetBlogPostQuery message, CancellationToken cancellationToken)
        {
            var viewModel = await _cacheService.GetOrAddAsync(() => CreateViewModelAsync(message.Id), message.Id);

            if (viewModel == null)
                return new NotFoundResult<BlogPostViewModel>();

            return viewModel.ToSuccessResult();
        }

        private async Task<BlogPostViewModel> CreateViewModelAsync(int blogPostId)
        {
            var blogPost = await GetBlogPost(blogPostId);

            if (blogPost == null)
                return null;

            var builder = new BlogPostViewModelBuilder();
            var links = await GetRelatedLinks(blogPostId);

            if (links != null && links.Count > 0)
                return builder.FromBlog(blogPost).WithRelatedLinks(links).Build();

            return builder.FromBlog(blogPost).Build();
        }

        private async Task<BlogPost> GetBlogPost(int id) 
            => await _context.BlogPosts.AsNoTracking()
                                       .Include(b => b.Author.AuthorProfile)
                                       .ThenInclude(ap => ap.Image)
                                       .Include(b => b.Tags)
                                       .Include(b => b.Image)
                                       .Include(b => b.ExternalLinks)
                                       .Include(b => b.Category)
                                       .FirstOrDefaultAsync(b => b.Id == id);

        private async Task<List<BlogPostLinkDto>> GetRelatedLinks(int id) 
            => await _context.BlogPosts.AsNoTracking()
                                       .Where(b => b.Id != id)
                                       .OrderByDescending(b => b.Id)
                                       .Take(10)
                                       .Select(b => 
                                            new BlogPostLinkDto()
                                            {
                                                Id = b.Id,
                                                Date = b.Date,
                                                Subject = b.Content.Subject,
                                                SubjectUrl = b.Content.SubjectUrl
                                            })
                                       .ToListAsync();
    }
}