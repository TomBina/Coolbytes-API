using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var blogPost = await _appDbContext.BlogPosts.Include(b => b.Tags).SingleOrDefaultAsync(b => b.Id == message.Id);
            blogPost.Update(message.Subject, message.ContentIntro, message.Content);

            if (message.Tags != null)
                blogPost.UpdateTags(message.Tags.Select(s => new BlogPostTag(s)));

            await _appDbContext.SaveChangesAsync();

            return Mapper.Map<BlogPostViewModel>(blogPost);
        }
    }
}