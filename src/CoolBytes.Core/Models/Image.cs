using CoolBytes.Core.Extensions;

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
            fileName.IsNotNullOrWhiteSpace();
            path.IsNotNullOrWhiteSpace();
            uriPath.IsNotNullOrWhiteSpace();
            contentType.IsNotNullOrWhiteSpace();

            FileName = fileName;
            Path = path;
            UriPath = uriPath;
            Length = length;
            ContentType = contentType;
        }

        private Image()
        {
            
        }
    }
}