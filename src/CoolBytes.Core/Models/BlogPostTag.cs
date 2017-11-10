using System;
using System.Collections.Generic;

namespace CoolBytes.Core.Models
{
    public class BlogPostTag
    {
        private sealed class NameEqualityComparer : IEqualityComparer<BlogPostTag>
        {
            public bool Equals(BlogPostTag x, BlogPostTag y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Name, y.Name);
            }

            public int GetHashCode(BlogPostTag obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public static IEqualityComparer<BlogPostTag> NameComparer { get; } = new NameEqualityComparer();

        public int Id { get; private set; }
        public string Name { get; private set; }
        public BlogPost BlogPost { get; private set; }

        public BlogPostTag(string name) 
            => Name = name.Trim() ?? throw new ArgumentNullException(nameof(name));

        private BlogPostTag()
        {
        }
    }
}