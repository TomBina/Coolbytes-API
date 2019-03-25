using AutoMapper;
using CoolBytes.WebAPI.Services.KeyVault;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;
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

            var keyVaultFactory = new KeyVaultSettingsFactory();
            var settings = keyVaultFactory.Create();
            builder.AddAzureKeyVault(settings.Vault, settings.ClientId, settings.Secret);

            return builder.Build();
        }

        private static void StartWebHost(string[] args, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(configuration)
                                .Enrich.FromLogContext()
                                .WriteTo.Console()
                                .WriteTo.MongoDB(configuration.GetConnectionString("MongoDb"))
                                .WriteTo.ApplicationInsights(configuration["coolbytesinstrumentationkey"], new EventTelemetryConverter())
                                .CreateLogger();

            try
            {
                Initialize(args, configuration);
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

        private static void Initialize(string[] args, IConfiguration configuration)
        {
            Mapper.Initialize(c => c.AddProfiles(typeof(Program).Assembly));
            Log.Information("Init db");
            DbSetup.InitDb(configuration);

            Log.Information("Starting web host");
            BuildWebHost(args, configuration).Run();
        }

        private static IWebHost BuildWebHost(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();
    }
}
