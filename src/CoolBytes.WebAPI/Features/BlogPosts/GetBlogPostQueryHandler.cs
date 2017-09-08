using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using MediatR;

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
            var blogPost = await _appDbContext.BlogPosts.FindAsync(message.Id);

            return Mapper.Map<BlogPostViewModel>(blogPost);
        }
    }
}