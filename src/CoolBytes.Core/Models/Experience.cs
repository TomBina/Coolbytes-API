using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoolBytes.Core.Models
{
    public class Experience
    {
        private sealed class IdEqualityComparer : IEqualityComparer<Experience>
        {
            public bool Equals(Experience x, Experience y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(Experience obj)
            {
                return obj.Id;
            }
        }

        public static IEqualityComparer<Experience> IdComparer { get; } = new IdEqualityComparer();

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Color { get; private set; }
        public Image Image { get; private set; }
        public int ImageId { get; set; }
        public AuthorProfile AuthorProfile { get; private set; }
        
        public Experience(int? id, string name, string color, Image image)
        {
            if (id != null)
                Id = (int) id;

            Assign(name, color, image);
        }

        public void Update(string name, string color, Image image) 
            => Assign(name, color, image);

        private void Assign(string name, string color, Image image)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Color = color ?? throw new ArgumentNullException(nameof(color));
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }

        private Experience()
        {
            
        }
    }
}