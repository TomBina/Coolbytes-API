
using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Categories.ViewModels;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostsOverviewViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
    }
}