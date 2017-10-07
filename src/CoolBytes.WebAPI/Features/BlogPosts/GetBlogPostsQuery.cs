using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class GetBlogPostsQuery : IRequest<IEnumerable<BlogPostSummaryViewModel>>
    {
        
    }
}
