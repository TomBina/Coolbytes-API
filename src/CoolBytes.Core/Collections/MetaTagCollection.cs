using CoolBytes.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace CoolBytes.Core.Collections
{
    public class MetaTagCollection : UpdatableCollection<MetaTag>
    {
        public override void Update(IEnumerable<MetaTag> items)
        {
            var itemsRemoved = Items.Except(items, MetaTag.NameValueComparer).ToArray();

            RemoveRange(itemsRemoved);
            AddRange(items);
        }

        public override bool Exists(MetaTag item) 
            => Items.Contains(item, MetaTag.NameValueComparer);
    }
}
