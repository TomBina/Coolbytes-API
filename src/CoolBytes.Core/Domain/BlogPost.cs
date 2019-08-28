using System;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Collections;

namespace CoolBytes.Core.Domain
{
    public class BlogPost : ISortable
    {
        public int Id { get; private set; }
        public DateTime Date { get; internal set; }
        public Author Author { get; private set; }
        public int AuthorId { get; private set; }
        public Image Image { get; private set; }
        public int? ImageId { get; private set; }
        public UpdatableCollection<BlogPostTag> Tags { get; private set; }
        public BlogPostContent Content { get; private set; }
        public UpdatableCollection<ExternalLink> ExternalLinks { get; private set; }
        public Category Category { get; private set; }
        public int CategoryId { get; private set; }
        public UpdatableCollection<MetaTag> MetaTags { get; private set; }
        public int SortOrder { get; private set; }

        private BlogPost()
        {
            Tags = new BlogPostTagCollection();
            ExternalLinks = new ExternalLinkCollection();
            MetaTags = new MetaTagCollection();
        }

        public BlogPost(BlogPostContent content, Author author, Category category)
        {
            Date = DateTime.Now;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            Tags = new BlogPostTagCollection();
            ExternalLinks = new ExternalLinkCollection();
            MetaTags = new MetaTagCollection();
            Category = category ?? throw new ArgumentNullException(nameof(category));
        }

        public void SetImage(Image image)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }

        public void SetCategory(Category category)
        {
            if (category?.Id != Category.Id)
                Category = category;
        }

        public void SetSortOrder(int sortOrder)
        {
            if (sortOrder <= 0) throw new ArgumentOutOfRangeException(nameof(sortOrder));
            SortOrder = sortOrder;
        }
    }
}