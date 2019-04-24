using System.IO;

namespace CoolBytes.Core.Abstractions
{
    public interface IImageFactoryValidator
    {
        bool Validate(Stream stream, string contentType);
    }
}