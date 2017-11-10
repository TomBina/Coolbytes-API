using System.Threading.Tasks;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class DeleteBlogPostCommandHandler : IAsyncRequestHandler<DeleteBlogPostCommand>
    {
        private readonly AppDbContext _context;

        public DeleteBlogPostCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteBlogPostCommand message) => await Delete(message.Id);

        private async Task Delete(int blogPostId)
        {
            var blogPost = await _context.BlogPosts.FindAsync(blogPostId);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
        }
    }
}