using System;
using System.Collections.Generic;

namespace CoolBytes.Core.Models
{
    public class ExternalLink
    {
        private sealed class NameEqualityComparer : IEqualityComparer<ExternalLink>
        {
            public bool Equals(ExternalLink x, ExternalLink y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Name, y.Name);
            }

            public int GetHashCode(ExternalLink obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public static IEqualityComparer<ExternalLink> NameComparer { get; } = new NameEqualityComparer();

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public BlogPost BlogPost { get; private set; }

        public ExternalLink(string name, string url)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }

        private ExternalLink()
        {
            
        }
    }
}