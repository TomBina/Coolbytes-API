using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Models
{
    public class Photo
    {
        public int Id { get; private set; }
        public string UriPath { get; private set; }
        public string FileName { get; private set; }
        public string Path { get; private set; }
        public long Length { get; private set; }
        public string ContentType { get; private set; }
        public Author Author { get; private set; }

        public Photo(string fileName, string path, string uriPath, long length, string contentType)
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

        private Photo()
        {
            
        }
    }
}