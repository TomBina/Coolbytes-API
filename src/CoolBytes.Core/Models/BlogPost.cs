using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Extensions;
using CoolBytes.Core.Factories;

namespace CoolBytes.Core.Models
{
    public class BlogPost
    {
        private readonly BlogPostTagCollection _tags = new BlogPostTagCollection();

        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime? Updated { get; private set; }
        public string Subject { get; private set; }
        public string ContentIntro { get; private set; }
        public string Content { get; private set; }
        public Author Author { get; private set; }
        public int AuthorId { get; private set; }
        public Photo Photo { get; private set; }
        public int? PhotoId { get; private set; }
        public IEnumerable<BlogPostTag> Tags { get => _tags; private set { } }

        public BlogPost(string subject, string contentInro, string content, Author author)
        {
            author.IsNotNull();
            subject.IsNotNullOrWhiteSpace();
            contentInro.IsNotNullOrWhiteSpace();
            content.IsNotNullOrWhiteSpace();

            Date = DateTime.Now;
            Subject = subject;
            ContentIntro = contentInro;
            Content = content;
            Author = author;
        }

        private BlogPost() { }

        public void Update(string subject, string contentIntro, string content)
        {
            subject.IsNotNullOrWhiteSpace();
            contentIntro.IsNotNullOrWhiteSpace();
            content.IsNotNullOrWhiteSpace();

            Updated = DateTime.Now;
            Subject = subject;
            ContentIntro = contentIntro;
            Content = content;
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

        public void SetPhoto(Photo photo)
        {
            photo.IsNotNull();

            Photo = photo;
        }
    }
}