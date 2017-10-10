using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public BlogPostBuilder WithTags(IEnumerable<string> tags)
        {
            _tags = tags?.Select(s => new BlogPostTag(s));

            return this;
        }

        public async Task<BlogPost> Build()
        {
            var author = await _author;
            var blogPost = new BlogPost(_blogPostContent, author);

            if (_tags != null)
                blogPost.AddTags(_tags);

            if (_image == null)
                return blogPost;

            var image = await _image();
            blogPost.SetImage(image);

            return blogPost;
        }
    }
}