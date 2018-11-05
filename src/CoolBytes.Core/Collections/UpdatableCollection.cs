using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CoolBytes.Core.Collections
{
    public abstract class UpdatableCollection<T> : Collection<T>
    {
        protected override void InsertItem(int index, T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            if (!Exists(item))
                base.InsertItem(index, item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        public abstract void Update(IEnumerable<T> items);
        public abstract bool Exists(T item);    
    }
}
