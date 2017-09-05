using System.IO;
using System.Threading.Tasks;
using CoolBytes.Core.Models;

namespace CoolBytes.Core.Interfaces
{
    public interface IPhotoFactory
    {
        Task<Photo> Create(Stream stream, string currentFileName, string contentType);
    }
}