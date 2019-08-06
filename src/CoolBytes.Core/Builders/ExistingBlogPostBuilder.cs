using CoolBytes.Core.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Builders
{
    public class ExistingBlogPostBuilder
    {
        private readonly IImageService _imageService;
        private BlogPost _blogPost;
        private Func<Task<Image>> _image;

        public ExistingBlogPostBuilder(IImageService imageService)
        {
            _imageService = imageService;
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
                    return await _imageService.Save(stream, file.FileName, file.ContentType);
            };

            return this;
        }

        public ExistingBlogPostBuilder WithTags(IEnumerable<BlogPostTag> tags)
        {
            When.NotNull(tags, () => _blogPost.Tags.Update(tags));

            return this;
        }

        public ExistingBlogPostBuilder WithExternalLinks(IEnumerable<ExternalLink> links)
        {
            When.NotNull(links, () => _blogPost.ExternalLinks.Update(links));

            return this;
        }

        public ExistingBlogPostBuilder WithCategory(Category category)
        {
            When.NotNull(category, () => _blogPost.SetCategory(category));

            return this;
        }
        public ExistingBlogPostBuilder WithMetaTags(IEnumerable<MetaTag> metaTags)
        {
            When.NotNull(metaTags, () => _blogPost.MetaTags.Update(metaTags));

            return this;
        }

        public async Task<BlogPost> Build()
        {
            await When.NotNull(_image, async () => _blogPost.SetImage(await _image()));

            return _blogPost;
        }
    }
}