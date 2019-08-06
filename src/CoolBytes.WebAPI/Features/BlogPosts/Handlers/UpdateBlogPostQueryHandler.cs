using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class UpdateBlogPostQueryHandler : IRequestHandler<UpdateBlogPostQuery, BlogPostUpdateViewModel>
    {
        private readonly HandlerContext<BlogPostUpdateViewModel> _context;
        private readonly AppDbContext _dbContext;

        public UpdateBlogPostQueryHandler(HandlerContext<BlogPostUpdateViewModel> context)
        {
            _context = context;
            _dbContext = context.DbContext;
        }

        public async Task<BlogPostUpdateViewModel> Handle(UpdateBlogPostQuery query, CancellationToken cancellationToken)
        {
            var blogPost = await _dbContext.BlogPosts.Include(b => b.Tags)
                                                     .Include(b => b.Image)
                                                     .Include(b => b.ExternalLinks)
                                                     .Include(b => b.MetaTags)
                                                     .FirstOrDefaultAsync(b => b.Id == query.Id);

            return _context.Map(blogPost);
        }
    }
}