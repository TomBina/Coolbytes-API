using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //DbSetup.SeedDb();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("mailgunsettings.json"))
                .Build();
    }
}
