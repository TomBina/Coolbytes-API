using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class GetBlogPostQueryHandler : IAsyncRequestHandler<GetBlogPostQuery, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public GetBlogPostQueryHandler(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
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
                .Include(b => b.Photo)
                .SingleAsync(b => b.Id == message.Id);
            return blogPost;
        }

        private BlogPostViewModel CreateViewModel(BlogPost blogPost)
        {
            var viewModel = Mapper.Map<BlogPostViewModel>(blogPost);
            viewModel.Photo?.FormatPhotoUri(_configuration);

            return viewModel;
        }
    }
}