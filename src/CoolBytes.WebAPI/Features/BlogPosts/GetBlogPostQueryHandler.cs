using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class GetBlogPostQueryHandler : IAsyncRequestHandler<GetBlogPostQuery, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;

        public GetBlogPostQueryHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BlogPostViewModel> Handle(GetBlogPostQuery message)
        {
            var blogPost = await GetBlogPost(message);

            return CreateViewModel(blogPost);
        }

        private async Task<BlogPost> GetBlogPost(GetBlogPostQuery message)
        {
            var blogPost = await _appDbContext.BlogPosts.AsNoTracking()
                .Include(b => b.Author.AuthorProfile)
                .Include(b => b.Tags)
                .Include(b => b.Image)
                .FirstOrDefaultAsync(b => b.Id == message.Id);
            return blogPost;
        }

        private BlogPostViewModel CreateViewModel(BlogPost blogPost) => Mapper.Map<BlogPostViewModel>(blogPost);
    }
}