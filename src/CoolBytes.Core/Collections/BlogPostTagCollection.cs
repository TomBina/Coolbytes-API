using System;
using System.Collections.Generic;
using System.Linq;
using CoolBytes.Core.Models;

namespace CoolBytes.Core.Collections
{
    public class BlogPostTagCollection : UpdatableCollection<BlogPostTag>
    {
        public override void Update(IEnumerable<BlogPostTag> items)
        {
            var itemsRemoved = Enumerable.Except(Items, items, BlogPostTag.NameComparer).ToArray();

            RemoveRange(itemsRemoved);
            AddRange(items);
        }

        public override bool Exists(BlogPostTag item) => 
            Enumerable.Any<BlogPostTag>(this.Items, b => b.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
    }
}