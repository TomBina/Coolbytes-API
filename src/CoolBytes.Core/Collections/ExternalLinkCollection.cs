using CoolBytes.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolBytes.Core.Collections
{
    public class ExternalLinkCollection : UpdatableCollection<ExternalLink>
    {
        public override void Update(IEnumerable<ExternalLink> items)
        {
            var itemsRemoved = Items.Except(items, ExternalLink.NameComparer).ToArray();

            RemoveRange(itemsRemoved);
            AddRange(items);
        }

        public override bool Exists(ExternalLink item)
            => Items.Any(externalLink => externalLink.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
    }
}
