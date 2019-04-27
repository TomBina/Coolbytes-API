using CoolBytes.Core.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Builders
{
    public class BlogPostBuilder
    {
        private readonly IAuthorService _authorService;
        private readonly IImageService _imageService;

        private BlogPostContent _blogPostContent;
        private Task<Author> _author;
        private Func<Task<Image>> _image;
        private IEnumerable<BlogPostTag> _tags;
        private IEnumerable<ExternalLink> _links;
        private Category _category;

        public BlogPostBuilder(IAuthorService authorService, IImageService imageService)
        {
            _authorService = authorService;
            _imageService = imageService;
        }

        public BlogPostBuilder WrittenByCurrentAuthor()
        {
            _author = _authorService.GetAuthor();

            return this;
        }

        public BlogPostBuilder WithContent(IBlogPostContent content)
        {
            _blogPostContent = new BlogPostContent(content.Subject, content.ContentIntro, content.Content);

            return this;
        }

        public BlogPostBuilder WithImage(IImageFile file)
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

        public BlogPostBuilder WithTags(IEnumerable<BlogPostTag> tags)
        {
            _tags = tags;

            return this;
        }

        public BlogPostBuilder WithExternalLinks(IEnumerable<ExternalLink> links)
        {
            _links = links;

            return this;
        }

        public BlogPostBuilder WithCategory(Category category)
        {
            _category = category;

            return this;
        }

        public async Task<BlogPost> Build()
        {
            var author = await _author;
            var blogPost = new BlogPost(_blogPostContent, author, _category);

            await When.NotNull(_image, async () => blogPost.SetImage(await _image()) );
            When.NotNull(_tags, () => blogPost.Tags.AddRange(_tags));
            When.NotNull(_links, () => blogPost.ExternalLinks.AddRange(_links));
            
            return blogPost;
        }
    }
}