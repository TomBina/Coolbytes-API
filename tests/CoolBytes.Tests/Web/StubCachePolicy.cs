using CoolBytes.Services.Caching;
using System.Threading.Tasks;

namespace CoolBytes.Tests.Web
{
    public class StubCachePolicy : ICachePolicy
    {
        public Task<bool> IsCacheActiveAsync() 
            => Task.FromResult(true);
    }
}