using System.Collections.Generic;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;

namespace CoolBytes.WebAPI.Features.Categories.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public List<BlogPostSummaryViewModel> BlogPosts { get; set; }
    }
}