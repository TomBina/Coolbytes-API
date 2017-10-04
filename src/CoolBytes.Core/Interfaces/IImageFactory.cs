using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Models;

namespace CoolBytes.Core.Interfaces
{
    public interface IImageFactory
    {
        Task<Image> Create(Stream stream, string currentFileName, string contentType);
    }
}