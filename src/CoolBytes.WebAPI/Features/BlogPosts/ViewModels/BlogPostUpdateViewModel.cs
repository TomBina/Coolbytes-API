using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using CoolBytes.WebAPI.Features.Images;

namespace CoolBytes.WebAPI.Features.BlogPosts.ViewModels
{
    public class BlogPostUpdateViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Updated { get; set; }
        public string Subject { get; set; }
        public string ContentIntro { get; set; }
        public string Content { get; set; }
        public IEnumerable<BlogPostTagDto> Tags { get; set; }
        public ImageViewModel Image { get; set; }
    }
}
