using System;

namespace CoolBytes.Core.Models
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Category(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
