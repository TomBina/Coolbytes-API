using System.IO;

namespace CoolBytes.Core.Interfaces
{
    public interface IPhotoValidator
    {
        bool Validate(Stream stream, string contentType);
    }
}