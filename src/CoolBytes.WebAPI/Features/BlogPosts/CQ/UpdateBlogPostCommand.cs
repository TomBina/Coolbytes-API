using System.Collections.Generic;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Builders;
using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using CoolBytes.WebAPI.Features.BlogPosts.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class UpdateBlogPostCommand : IRequest<BlogPostSummaryViewModel>, IBlogPostContent
    {
        private IFormFile _file;
        public int Id { get; set; }
        public string Subject { get; set; }
        public string ContentIntro { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IImageFile ImageFile { get; set; }
        public IFormFile File
        {
            get => _file;
            set
            {
                _file = value;

                var imageFile = new ImageFile()
                {
                    ContentType = _file?.ContentType,
                    FileName = _file?.FileName,
                    OpenStream = () => _file?.OpenReadStream()
                };

                ImageFile = imageFile;
            }
        }
        public IEnumerable<ExternalLinkDto> ExternalLinks { get; set; }
        public int CategoryId { get; set; }
    }
}
