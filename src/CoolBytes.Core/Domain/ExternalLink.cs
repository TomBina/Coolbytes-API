using System;
using System.Collections.Generic;

namespace CoolBytes.Core.Domain
{
    public class ExternalLink
    {
        private sealed class NameUrlEqualityComparer : IEqualityComparer<ExternalLink>
        {
            public bool Equals(ExternalLink x, ExternalLink y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(x.Url, y.Url, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(ExternalLink obj)
            {
                unchecked
                {
                    return (StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj.Name) * 397) ^ StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj.Url);
                }
            }
        }

        public static IEqualityComparer<ExternalLink> NameUrlComparer { get; } = new NameUrlEqualityComparer();

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