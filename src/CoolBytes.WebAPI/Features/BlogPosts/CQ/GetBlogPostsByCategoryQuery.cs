using System.Collections.Generic;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class GetBlogPostsByCategoryQuery : IRequest<IEnumerable<BlogPostSummaryViewModel>>
    {
        public int CategoryId { get; set; }
    }
}