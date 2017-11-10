using System;

namespace CoolBytes.Core.Models
{
    public class Image
    {
        public int Id { get; private set; }
        public string UriPath { get; private set; }
        public string FileName { get; private set; }
        public string Path { get; private set; }
        public long Length { get; private set; }
        public string ContentType { get; private set; }

        public Image(string fileName, string path, string uriPath, long length, string contentType)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            UriPath = uriPath ?? throw new ArgumentNullException(nameof(uriPath));
            Length = length;
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }

        private Image()
        {
            
        }
    }
}