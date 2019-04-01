using CoolBytes.Core.Attributes;

namespace CoolBytes.Services.Environment
{
    [Scoped]
    public class EnvironmentService : IEnvironmentService
    {
        public string GetVariable(string key)
            => System.Environment.GetEnvironmentVariable(key);
    }
}
