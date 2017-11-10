using System;
using System.Text.RegularExpressions;

namespace CoolBytes.Core.Models
{
    public class BlogPostContent
    {
        private string _subjectUrl;

        public DateTime? Updated { get; private set; }
        public string Subject { get; private set; }
        public string SubjectUrl
        {
            get => _subjectUrl;
            private set
            {
                _subjectUrl = value.Replace(' ', '-').ToLower();
                _subjectUrl = Regex.Replace(_subjectUrl, @"[^\w-]", string.Empty);
            }
        }
        public string ContentIntro { get; private set; }
        public string Content { get; private set; }

        public BlogPostContent(string subject, string contentIntro, string content)
        {
            Validate(subject, contentIntro, content);

            Subject = subject;
            SubjectUrl = subject;
            ContentIntro = contentIntro;
            Content = content;
        }

        private BlogPostContent()
        {
        }

        public BlogPostContent Update(string subject, string contentIntro, string content)
        {
            Validate(subject, contentIntro, content);

            Updated = DateTime.Now;
            Subject = subject;
            SubjectUrl = subject;
            ContentIntro = contentIntro;
            Content = content;

            return this;
        }

        private static void Validate(string subject, string contentIntro, string content)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (contentIntro == null) throw new ArgumentNullException(nameof(contentIntro));
            if (content == null) throw new ArgumentNullException(nameof(content));
        }
    }
}