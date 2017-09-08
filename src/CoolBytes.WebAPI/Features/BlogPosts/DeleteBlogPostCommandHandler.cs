using System.Threading.Tasks;
using CoolBytes.Data;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class DeleteBlogPostCommandHandler : IAsyncRequestHandler<DeleteBlogPostCommand>
    {
        private readonly AppDbContext _appDbContext;

        public DeleteBlogPostCommandHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Handle(DeleteBlogPostCommand message)
        {
            var blogPost = await _appDbContext.BlogPosts.FindAsync(message.Id);
            _appDbContext.BlogPosts.Remove(blogPost);
            await _appDbContext.SaveChangesAsync();
        }
    }
}