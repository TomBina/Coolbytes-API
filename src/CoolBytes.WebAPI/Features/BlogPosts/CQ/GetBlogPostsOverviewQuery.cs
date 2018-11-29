using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class GetBlogPostsOverviewQuery : IRequest<BlogPostsOverviewViewModel>
    {
    }
}
