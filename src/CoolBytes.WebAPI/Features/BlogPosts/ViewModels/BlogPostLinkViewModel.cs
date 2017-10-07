using System;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostLinkViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
    }
}
