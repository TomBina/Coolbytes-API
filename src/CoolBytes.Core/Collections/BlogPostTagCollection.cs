using System;
using System.Collections.Generic;
using System.Linq;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Collections
{
    public class BlogPostTagCollection : UpdatableCollection<BlogPostTag>
    {
        public override void Update(IEnumerable<BlogPostTag> items)
        {
            var itemsRemoved = Items.Except(items, BlogPostTag.NameComparer).ToArray();

            RemoveRange(itemsRemoved);
            AddRange(items);
        }

        public override bool Exists(BlogPostTag item) => 
            Items.Any(b => b.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
    }
}