using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoolBytes.Core.Builders
{
    public class ExistingBlogPostBuilder 
    {
        private readonly IImageFactory _imageFactory;

        private BlogPost _blogPost;
        private Func<Task<Image>> _image;

        public ExistingBlogPostBuilder(IImageFactory imageFactory)
        {
            _imageFactory = imageFactory;
        }

        public ExistingBlogPostBuilder UseBlogPost(BlogPost blogPost)
        {
            _blogPost = blogPost;

            return this;
        }

        public ExistingBlogPostBuilder WithContent(IBlogPostContent content)
        {
            _blogPost.Update(content.Subject, content.ContentIntro, content.Content);

            return this;
        }

        public ExistingBlogPostBuilder WithImage(IImageFile file)
        {
            if (file == null)
                return this;

            _image = async () =>
            {
                using (var stream = file.OpenStream())
                    return await _imageFactory.Create(stream, file.FileName, file.ContentType);
            };

            return this;
        }

        public ExistingBlogPostBuilder WithTags(IEnumerable<string> tags)
        {
            if (tags != null)
                _blogPost.UpdateTags(tags.Select(s => new BlogPostTag(s)));

            return this;
        }

        public async Task<BlogPost> Build()
        {
            if (_image == null)
                return _blogPost;

            var image = await _image();
            _blogPost.SetImage(image);

            return _blogPost;

        }
    }
}