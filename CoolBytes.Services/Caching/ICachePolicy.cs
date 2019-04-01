using System.Threading.Tasks;

namespace CoolBytes.Services.Caching
{
    public interface ICachePolicy
    {
        Task<bool> IsCacheActiveAsync();
    }
}
    