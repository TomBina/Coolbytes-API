using CoolBytes.WebAPI.Features.Images.ViewModels;
using System;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostSummaryViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string SubjectUrl { get; set; }
        public string ContentIntro { get; set; }
        public ImageViewModel Image { get; set; }
        public string AuthorName { get; set; }
        public string Category { get; set; }
    }
}