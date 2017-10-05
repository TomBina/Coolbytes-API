using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
                .Build();
    }
}
