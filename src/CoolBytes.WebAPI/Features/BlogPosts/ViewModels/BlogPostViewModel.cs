using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using CoolBytes.WebAPI.Features.Images;
using System;
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Updated { get; set; }
        public string Subject { get; set; }
        public string ContentIntro { get; set; }
        public string Content { get; set; }
        public IEnumerable<BlogPostTagDto> Tags { get; set; }
        public ImageViewModel Image { get; set; }
        public Author Author { get; set; }
        public IEnumerable<ExternalLinkDto> ExternalLinks { get; set; }
        public IEnumerable<BlogPostLinkDto> RelatedLinks { get; set; }
    }
}
