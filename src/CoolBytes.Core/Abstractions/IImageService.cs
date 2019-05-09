using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Abstractions
{
    public interface IImageService
    {
        Task Delete(Image image);
        Task<Image> Save(Stream stream, string currentFileName, string contentType);
    }
}
