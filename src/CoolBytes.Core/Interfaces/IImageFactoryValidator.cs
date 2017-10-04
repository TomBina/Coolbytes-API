using System.IO;

namespace CoolBytes.Core.Interfaces
{
    public interface IImageFactoryValidator
    {
        bool Validate(Stream stream, string contentType);
    }
}