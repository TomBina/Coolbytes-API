using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class UpdateBlogPostQuery : IRequest<BlogPostUpdateViewModel>
    {
        public int Id { get; set; }
    }
}