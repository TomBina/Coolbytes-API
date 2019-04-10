using System;
using System.Collections.Generic;
using System.Linq;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class CreatedImages
    {
        private List<string> _createdImages = new List<string>();

        public void AddImage(Image image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            _createdImages.Add(image.Path);
        }

        public void AddRange(IEnumerable<Image> images)
        {
            if (images == null) throw new ArgumentNullException(nameof(images));

            _createdImages.AddRange(images.Select(i => i.Path));
        }

        public IEnumerable<string> GetAll() => _createdImages;
    }
}
