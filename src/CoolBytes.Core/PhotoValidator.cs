using System.Collections.Generic;
using System.IO;
using CoolBytes.Core.Interfaces;

namespace CoolBytes.Core
{
    public class PhotoValidator : IPhotoValidator
    {
        private const int MaxFileSize = (1024 * 1024) * 3;
        private readonly List<string> _allowedContentTypes;

        public PhotoValidator()
        {
            _allowedContentTypes = new List<string>
            {
                "image/jpeg",
                "image/jpg",
                "image/png"
            };
        }

        public bool Validate(Stream stream, string contentType) => 
            _allowedContentTypes.Contains(contentType) && (stream.Length <= MaxFileSize && stream.Length > 0);
    }
}