using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class GetBlogPostsQuery : IRequest<IEnumerable<BlogPostSummaryViewModel>>
    {
        public string Tag { get; set; }
    }
}
