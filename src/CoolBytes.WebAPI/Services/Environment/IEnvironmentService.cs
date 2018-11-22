namespace CoolBytes.WebAPI.Services.Environment
{
    public interface IEnvironmentService
    {
        string GetVariable(string key);
    }
}