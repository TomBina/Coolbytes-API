using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoolBytes.Core.Extensions;
using CoolBytes.Core.Factories;

namespace CoolBytes.Core.Models
{
    public class BlogPost
    {
        private readonly BlogPostTagCollection _tags = new BlogPostTagCollection();
        private string _subject, _subjectUrl, _contentIntro, _content;
        private Author _author;

        public int Id { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime? Updated { get; private set; }
        public string Subject
        {
            get => _subject;
            internal set
            {
                value.IsNotNullOrWhiteSpace();
                _subject = value;
                SubjectUrl = value;
            }
        }
        public string SubjectUrl
        {
            get => _subjectUrl;
            private set
            {
                _subjectUrl = value.Replace(' ', '-').ToLower();
                _subjectUrl = Regex.Replace(_subjectUrl, @"[^\w-]", string.Empty);
            }
        }
        public string ContentIntro
        {
            get => _contentIntro;
            internal set
            {
                value.IsNotNullOrWhiteSpace();
                _contentIntro = value;
            }
        }
        public string Content
        {
            get => _content;
            internal set
            {
                value.IsNotNullOrWhiteSpace();
                _content = value;
            }
        }
        public Author Author
        {
            get => _author;
            internal set
            {
                value.IsNotNull();
                _author = value;
            }
        }
        public int AuthorId { get; private set; }
        public Image Image { get; private set; }
        public int? ImageId { get; private set; }
        public IEnumerable<BlogPostTag> Tags { get => _tags; private set { } }

        internal BlogPost() { }

        public BlogPost(string subject, string contentIntro, string content, Author author)
        {
            Date = DateTime.Now;
            Subject = subject;
            ContentIntro = contentIntro;
            Content = content;
            Author = author;
        }

        public void Update(string subject, string contentIntro, string content)
        {
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

        public void SetImage(Image image)
        {
            image.IsNotNull();

            Image = image;
        }
    }
}