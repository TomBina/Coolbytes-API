using System;
using System.Collections.Generic;
using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Models
{
    public class BlogPost
    {
        private readonly List<BlogPostTag> _tags = new List<BlogPostTag>();

        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime Updated { get; private set; }
        public string Subject { get; private set; }
        public string Content { get; private set; }
        public Author Author { get; private set; }
        public IEnumerable<BlogPostTag> Tags { get => _tags; private set { } }

        public BlogPost(string subject, string content, Author author)
        {
            author.IsNotNull();
            subject.IsNotNullOrWhiteSpace();
            content.IsNotNullOrWhiteSpace();
            
            Date = DateTime.Now;
            Subject = subject;
            Content = content;
            Author = author;
        }

        private BlogPost() { }

        public void Update(string subject, string content)
        {
            subject.IsNotNullOrWhiteSpace();
            content.IsNotNullOrWhiteSpace();

            Updated = DateTime.Now;
            Subject = subject;
            Content = content;
        }

        public void AddTag(BlogPostTag blogPostTag)
        {
            _tags.Add(blogPostTag);
        }

        public void AddTags(IEnumerable<BlogPostTag> tags)
        {
            _tags.AddRange(tags);
        }
    }
}