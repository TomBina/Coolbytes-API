using System;
using System.Collections.Generic;
using CoolBytes.Core.Models;
using Microsoft.Extensions.Configuration;
using Photo = CoolBytes.WebAPI.ViewModels.Photo;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class BlogPostViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Updated { get; set; }
        public string Subject { get; set; }
        public string SubjectUrl { get; set; }
        public string ContentIntro { get; set; }
        public string Content { get; set; }
        public IEnumerable<BlogPostTagViewModel> Tags { get; set; }
        public string AuthorName { get; set; }
        public Photo Photo { get; set; }
    }
}