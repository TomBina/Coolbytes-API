using CoolBytes.Core.Attributes;

namespace CoolBytes.Services.Environment
{
    [Inject(typeof(IEnvironmentService))]
    public class EnvironmentService : IEnvironmentService
    {
        public string GetVariable(string key)
            => System.Environment.GetEnvironmentVariable(key);
    }
}
