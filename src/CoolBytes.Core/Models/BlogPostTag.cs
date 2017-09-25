using System;
using System.Collections.Generic;

namespace CoolBytes.Core.Models
{
    public class BlogPostTag
    {
        private sealed class BlogPostTagEqualityComparer : IEqualityComparer<BlogPostTag>
        {
            public bool Equals(BlogPostTag x, BlogPostTag y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(BlogPostTag obj)
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
            }
        }

        public static IEqualityComparer<BlogPostTag> EqualityComparer { get; } = new BlogPostTagEqualityComparer();

        public int Id { get; private set; }
        public string Name { get; private set; }
        public BlogPost BlogPost { get; private set; }

        public BlogPostTag(string name)
        {
            Name = name;
        }

        private BlogPostTag()
        {
        }
    }
}