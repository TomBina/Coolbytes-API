using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using CoolBytes.WebAPI.Utils;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class GetBlogPostQuery : IRequest<Result<BlogPostViewModel>>
    {
        public int Id { get; set; }
    }
}
