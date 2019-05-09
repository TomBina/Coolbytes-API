using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace CoolBytes.Services.BlobStorage
{
    public interface IBlobClient
    {
        CloudBlockBlob Get(string name);
        CloudBlockBlob Create(string name);
        Task Delete(string name);
    }
}
