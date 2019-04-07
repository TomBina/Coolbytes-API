using System.Collections.Generic;
using CoolBytes.Tests.Core;

namespace CoolBytes.Core.Models
{
    public class Resorter
    {
        public void Sort(IList<ISortable> sortables, int[] newSortOrder)
        {
            for (var i = 0; i < sortables.Count; i++)
            {
                sortables[i].SetSortOrder(newSortOrder[i]);
            }
        }
    }
}