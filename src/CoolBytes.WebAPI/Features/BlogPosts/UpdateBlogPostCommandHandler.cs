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
    public class UpdateBlogPostCommandHandler : IAsyncRequestHandler<UpdateBlogPostCommand, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public UpdateBlogPostCommandHandler(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }

        public async Task<BlogPostViewModel> Handle(UpdateBlogPostCommand message)
        {
            var blogPost = await GetBlogPost(message);

            await UpdateBlogPost(message, blogPost);

            return CreateViewModel(blogPost);
        }

        private async Task<BlogPost> GetBlogPost(UpdateBlogPostCommand message)
        {
            var blogPost = await _appDbContext.BlogPosts.Include(b => b.Tags).SingleOrDefaultAsync(b => b.Id == message.Id);
            return blogPost;
        }

        private async Task UpdateBlogPost(UpdateBlogPostCommand message, BlogPost blogPost)
        {
            blogPost.Update(message.Subject, message.ContentIntro, message.Content);

            if (message.Tags != null)
                blogPost.UpdateTags(message.Tags.Select(s => new BlogPostTag(s)));

            await _appDbContext.SaveChangesAsync();
        }
        private BlogPostViewModel CreateViewModel(BlogPost blogPost)
        {
            var viewModel = Mapper.Map<BlogPostViewModel>(blogPost);
            viewModel.Photo?.FormatPhotoUri(_configuration);

            return viewModel;
        }

    }
}