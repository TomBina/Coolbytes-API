using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class DeleteBlogPostCommandHandler : AsyncRequestHandler<DeleteBlogPostCommand>
    {
        private readonly AppDbContext _context;

        public DeleteBlogPostCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        protected override async Task Handle(DeleteBlogPostCommand message, CancellationToken cancellationToken) => await Delete(message.Id);

        private async Task Delete(int blogPostId)
        {
            var blogPost = await _context.BlogPosts.FindAsync(blogPostId);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
        }
    }
}