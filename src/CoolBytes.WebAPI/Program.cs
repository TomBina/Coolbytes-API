using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace CoolBytes.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            StartWebHost(args, configuration);
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("mailgunsettings.json");

            var currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            builder.AddJsonFile($"appsettings.{currentEnvironment}.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        private static void StartWebHost(string[] args, IConfiguration configuration)
        {
            Mapper.Initialize(c => c.AddProfiles(typeof(Program).Assembly));
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.MongoDB(configuration.GetConnectionString("MongoDb"))
                .CreateLogger();

            try
            {
                Log.Information("Init db");
                DbSetup.InitDb(configuration);

                Log.Information("Starting web host");
                BuildWebHost(args, configuration).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Start failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();
    }
}
