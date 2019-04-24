using System.Collections.Generic;
using System.Linq;
using CoolBytes.Core.Abstractions;

namespace CoolBytes.Core.Domain
{
    public class Sorter<T> where T : ISortable
    {
        public void Sort(IEnumerable<T> sortables, IList<int> newSortOrder)
        {
            for (var i = 0; i < newSortOrder.Count; i++)
            {
                sortables.First(s => s.Id == newSortOrder[i]).SetSortOrder(i + 1);
            }
        }
    }
}