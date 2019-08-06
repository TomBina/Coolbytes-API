using System;
using System.Collections.Generic;

namespace CoolBytes.Core.Domain
{
    public class MetaTag
    {
        private sealed class NameValueEqualityComparer : IEqualityComparer<MetaTag>
        {
            public bool Equals(MetaTag x, MetaTag y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Name, y.Name, StringComparison.CurrentCultureIgnoreCase) && string.Equals(x.Value, y.Value, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(MetaTag obj)
            {
                unchecked
                {
                    return (StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj.Name) * 397) ^ StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj.Value);
                }
            }
        }

        public static IEqualityComparer<MetaTag> NameValueComparer { get; } = new NameValueEqualityComparer();

        public int Id { get; private set; }
        public string Name { get; }
        public string Value { get; }

        public MetaTag(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
