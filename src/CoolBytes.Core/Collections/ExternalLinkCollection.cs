using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;

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
            => this.Items.Any(externalLink => externalLink.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
    }
}
