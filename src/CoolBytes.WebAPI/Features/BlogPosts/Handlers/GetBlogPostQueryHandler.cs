using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class GetBlogPostQueryHandler : IRequestHandler<GetBlogPostQuery, BlogPostViewModel>
    {
        private readonly AppDbContext _context;

        public GetBlogPostQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPostViewModel> Handle(GetBlogPostQuery message, CancellationToken cancellationToken) 
            => await ViewModel(message.Id);

        private async Task<BlogPostViewModel> ViewModel(int blogPostId)
        {
            var builder = new BlogPostViewModelBuilder();
            var blogPost = await GetBlogPost(blogPostId);
            var links = await GetRelatedLinks(blogPostId);

            if (links != null && links.Count > 0)
                return builder.FromBlog(blogPost).WithRelatedLinks(links).Build();
            else
                return builder.FromBlog(blogPost).Build();
        }

        private async Task<BlogPost> GetBlogPost(int id) 
            => await _context.BlogPosts.AsNoTracking()
                                       .Include(b => b.Author.AuthorProfile)
                                       .ThenInclude(ap => ap.Image)
                                       .Include(b => b.Tags)
                                       .Include(b => b.Image)
                                       .Include(b => b.ExternalLinks)
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