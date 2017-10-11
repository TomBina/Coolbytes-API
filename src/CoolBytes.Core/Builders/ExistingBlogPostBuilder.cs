using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolBytes.Core.Utils;

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
            _blogPost.Content.Update(content.Subject, content.ContentIntro, content.Content);

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
            When.NotNull(tags, () => _blogPost.Tags.Update(tags.Select(s => new BlogPostTag(s))));
            
            return this;
        }

        public ExistingBlogPostBuilder WithExternalLinks(IEnumerable<ExternalLink> links)
        {
            When.NotNull(links, () => _blogPost.ExternalLinks.Update(links));

            return this;
        }

        public async Task<BlogPost> Build()
        {
            await When.NotNull(_image, async () => _blogPost.SetImage(await _image()));
            
            return _blogPost;
        }
    }
}