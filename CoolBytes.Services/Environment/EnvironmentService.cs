namespace CoolBytes.Services.Environment
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetVariable(string key)
            => System.Environment.GetEnvironmentVariable(key);
    }
}
