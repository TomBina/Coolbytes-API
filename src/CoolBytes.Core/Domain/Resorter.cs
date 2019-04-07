using System.Collections.Generic;
using CoolBytes.Core.Interfaces;

namespace CoolBytes.Core.Domain
{
    public class ReSorter<T> where T: ISortable
    {
        public void Sort(IList<T> sortables, IList<int> newSortOrder)
        {
            for (var i = 0; i < sortables.Count; i++)
            {
                sortables[i].SetSortOrder(newSortOrder[i]);
            }
        }
    }
}