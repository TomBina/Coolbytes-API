using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolBytes.Core.Utils;

namespace CoolBytes.Core.Builders
{
    public class BlogPostBuilder
    {
        private readonly IAuthorService _authorService;
        private readonly IImageFactory _imageFactory;

        private BlogPostContent _blogPostContent;
        private Task<Author> _author;
        private Func<Task<Image>> _image;
        private IEnumerable<BlogPostTag> _tags;
        private IEnumerable<ExternalLink> _links;

        public BlogPostBuilder(IAuthorService authorService, IImageFactory imageFactory)
        {
            _authorService = authorService;
            _imageFactory = imageFactory;
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
                    return await _imageFactory.Create(stream, file.FileName, file.ContentType);
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

        public async Task<BlogPost> Build()
        {
            var author = await _author;
            var blogPost = new BlogPost(_blogPostContent, author);

            await When.NotNull(_image, async () => blogPost.SetImage(await _image()) );
            When.NotNull(_tags, () => blogPost.Tags.AddRange(_tags));
            When.NotNull(_links, () => blogPost.ExternalLinks.AddRange(_links));
            
            return blogPost;
        }
    }
}