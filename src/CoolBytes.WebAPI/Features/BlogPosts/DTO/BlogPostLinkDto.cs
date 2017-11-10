using System;

namespace CoolBytes.WebAPI.Features.BlogPosts.DTO
{
    public class BlogPostLinkDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string SubjectUrl { get; set; }
        public DateTime Date { get; set; }
    }
}
