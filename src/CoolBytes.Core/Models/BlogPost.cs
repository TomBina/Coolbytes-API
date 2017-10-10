using CoolBytes.Core.Extensions;
using System;
using System.Collections.Generic;

namespace CoolBytes.Core.Models
{
    public class BlogPost
    {
        private readonly BlogPostTagCollection _tags = new BlogPostTagCollection();

        public int Id { get; private set; }
        public DateTime Date { get; internal set; }
        public Author Author { get; private set; }
        public int AuthorId { get; private set; }
        public Image Image { get; private set; }
        public int? ImageId { get; private set; }
        public IEnumerable<BlogPostTag> Tags { get => _tags; private set { } }
        public BlogPostContent Content { get; private set; }

        private BlogPost()
        {
        }

        public BlogPost(BlogPostContent content, Author author)
        {
            content.IsNotNull();
            author.IsNotNull();

            Date = DateTime.Now;
            Content = content;
            Author = author;
        }

        public BlogPost AddTag(BlogPostTag blogPostTag)
        {
            blogPostTag.IsNotNull();
            _tags.Add(blogPostTag);

            return this;
        }

        public BlogPost AddTags(IEnumerable<BlogPostTag> blogPostTags)
        {
            blogPostTags.IsNotNull();

            _tags.AddRange(blogPostTags);

            return this;
        }

        public BlogPost UpdateTags(IEnumerable<BlogPostTag> blogPostTags)
        {
            blogPostTags.IsNotNull();
            _tags.Update(blogPostTags);

            return this;
        }

        public void SetImage(Image image)
        {
            image.IsNotNull();

            Image = image;
        }
    }
}