using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class UpdateBlogPostCommandHandler : IAsyncRequestHandler<UpdateBlogPostCommand, BlogPostViewModel>
    {
        private readonly AppDbContext _appDbContext;

        public UpdateBlogPostCommandHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BlogPostViewModel> Handle(UpdateBlogPostCommand message)
        {
            var blogPost = await _appDbContext.BlogPosts.FindAsync(message.Id);

            blogPost.Update(message.Subject, message.ContentIntro, message.Content);

            if (message.BlogPostTags != null)
                blogPost.AddTags(message.BlogPostTags);

            await _appDbContext.SaveChangesAsync();

            return Mapper.Map<BlogPostViewModel>(blogPost);
        }
    }
}