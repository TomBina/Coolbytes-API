namespace CoolBytes.WebAPI.Services.Environment
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetVariable(string key)
            => System.Environment.GetEnvironmentVariable(key);
    }
}
