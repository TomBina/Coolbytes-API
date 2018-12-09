using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services.Caching
{
    public interface ICachePolicy
    {
        Task<bool> IsCacheActiveAsync();
    }
}
    