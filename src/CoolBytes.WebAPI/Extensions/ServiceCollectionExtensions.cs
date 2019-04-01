using CoolBytes.Services.Mailer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using CoolBytes.Core.Attributes;
using CoolBytes.Core.Interfaces;
using CoolBytes.Services.Caching;
using CoolBytes.WebAPI.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(o =>
            {
                o.AddPolicy("ProductionPolicy", builder => builder.WithOrigins("http://coolbytes.io", "http://www.coolbytes.io").AllowAnyMethod().AllowAnyHeader().Build());
                o.AddPolicy("DevPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build());
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = config["Auth0:Domain"];
                o.Audience = config["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("admin",
                    policy => policy.Requirements.Add(new HasScopeRequirement("admin", config["Auth0:Domain"])));
            });

            return services;
        }

        public static IServiceCollection ScanServices(this IServiceCollection services)
        {
            services.Scan(s =>
                s.FromAssemblyOf<ICacheService>()
                    .AddClasses(c => c.WithAttribute<ScopedAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.Scan(s =>
                s.FromAssemblyOf<IImageFactory>()
                    .AddClasses(c => c.WithAttribute<ScopedAttribute>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddMailgun(this IServiceCollection services)
        {
            services.AddSingleton<MailgunMailerOptionsFactory, MailgunMailerOptionsFactory>();
            services.AddScoped<IMailer>(sp =>
            {
                var optionsFactory = sp.GetService<MailgunMailerOptionsFactory>();
                var options = optionsFactory.Create();
                var httpClient = sp.GetService<IHttpClientFactory>();
                var logger = sp.GetService<ILogger<MailgunMailer>>();
                var sendValidator = sp.GetService<ISendValidator>();

                return new MailgunMailer(httpClient, options, sendValidator, logger);
            });

            return services;
        }
    }
}
