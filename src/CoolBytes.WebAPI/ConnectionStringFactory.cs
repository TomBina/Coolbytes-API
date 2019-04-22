using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI
{
    public class ConnectionStringFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public ConnectionStringFactory(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public string Create()
        {
            var environmentName = _environment.EnvironmentName;
            var connectionString = _configuration.GetConnectionString("Default");

            if (!environmentName.Contains("azure"))
                return connectionString;

            connectionString = connectionString.Replace("{USERID}", _configuration["sqluser"]);
            connectionString = connectionString.Replace("{PASSWORD}", _configuration["sqlpwd"]);

            return connectionString;
        }
    }
}