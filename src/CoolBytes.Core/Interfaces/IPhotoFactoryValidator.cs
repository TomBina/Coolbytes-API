using System.IO;

namespace CoolBytes.Core.Interfaces
{
    public interface IPhotoFactoryValidator
    {
        bool Validate(Stream stream, string contentType);
    }
}