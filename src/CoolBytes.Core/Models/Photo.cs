using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Models
{
    public class Photo
    {
        public int Id { get; private set; }
        public string FileName { get; private set; }
        public string Path { get; private set; }
        public long Length { get; private set; }
        public string ContentType { get; private set; }

        public Photo(string fileName, string path, long length, string contentType)
        {
            fileName.IsNotNullOrWhiteSpace();
            path.IsNotNullOrWhiteSpace();
            contentType.IsNotNullOrWhiteSpace();

            FileName = fileName;
            Path = path;
            Length = length;
            ContentType = contentType;
        }

        private Photo()
        {
            
        }
    }
}