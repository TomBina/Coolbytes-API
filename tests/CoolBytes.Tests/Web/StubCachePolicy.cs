using System.Threading.Tasks;
using CoolBytes.WebAPI.Services.Caching;

namespace CoolBytes.Tests.Web
{
    public class StubCachePolicy : ICachePolicy
    {
        public Task<bool> IsCacheActiveAsync() 
            => Task.FromResult(true);
    }
}