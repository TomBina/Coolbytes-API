using System;
using CoolBytes.Core.Abstractions;

namespace CoolBytes.Core.Domain
{
    public class Category : ISortable
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int SortOrder { get; private set; }
        public string Description { get; private set; }
        public bool IsCourse { get; private set; }

        public Category(int id, string name, int sortOrder, string description, bool isCourse = false) : this(name, sortOrder, description, isCourse) 
            => Id = id;

        public Category(string name, int sortOrder, string description, bool isCourse = false)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (sortOrder <= 0) throw new ArgumentOutOfRangeException(nameof(sortOrder));
            SortOrder = sortOrder;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IsCourse = isCourse;
        }

        public void UpdateName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void UpdateDescription(string description)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public void SetSortOrder(int sortOrder)
        {
            if (sortOrder <= 0) throw new ArgumentOutOfRangeException(nameof(sortOrder));
            SortOrder = sortOrder;
        }

        public void UpdateIsCourse(bool isCourse) 
            => IsCourse = isCourse;
    }
}
