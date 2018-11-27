using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public List<BlogPostSummaryViewModel> BlogPosts { get; set; }
    }
}