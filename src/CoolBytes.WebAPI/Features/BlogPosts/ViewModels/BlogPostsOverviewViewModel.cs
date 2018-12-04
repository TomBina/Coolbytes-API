
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostsOverviewViewModel
    {
        public List<CategoryBlogPostsViewModel> Categories { get; set; }
    }
}