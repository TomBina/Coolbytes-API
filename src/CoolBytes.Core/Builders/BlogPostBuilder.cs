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

        private BlogPost _blogPost;
        private Task<Author> _author;
        private Func<Task<Image>> _image;

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
            _blogPost = new BlogPost()
            {
                Subject = content.Subject,
                ContentIntro = content.ContentIntro,
                Content = content.Content
            };

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
            if (tags != null)
                _blogPost.AddTags(tags.Select(s => new BlogPostTag(s)));

            return this;
        }

        public async Task<BlogPost> Build()
        {
            var author = await _author;
            _blogPost.Author = author;

            if (_image == null)
                return _blogPost;

            var image = await _image();
            _blogPost.SetImage(image);

            return _blogPost;
        }
    }
}