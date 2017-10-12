using System.Collections.Generic;
using System.Linq;
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
    public class GetBlogPostQueryHandler : IAsyncRequestHandler<GetBlogPostQuery, BlogPostViewModel>
    {
        private readonly AppDbContext _context;

        public GetBlogPostQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPostViewModel> Handle(GetBlogPostQuery message) 
            => await ViewModel(message.Id);

        private async Task<BlogPostViewModel> ViewModel(int blogPostId)
        {
            var builder = new BlogPostViewModelBuilder(_context);
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
                                       .Include(b => b.Tags)
                                       .Include(b => b.Image)
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
                                                Subject = b.Content.Subject
                                            })
                                       .ToListAsync();
    }
}