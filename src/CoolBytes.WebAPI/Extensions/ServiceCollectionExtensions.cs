using CoolBytes.WebAPI.Services.Mailer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace CoolBytes.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMailgun(this IServiceCollection services)
        {
            services.AddSingleton<MailgunMailerOptionsFactory, MailgunMailerOptionsFactory>();
            services.AddScoped<IMailer>(sp =>
            {
                var optionsFactory = sp.GetService<MailgunMailerOptionsFactory>();
                var options = optionsFactory.Create();
                var httpClient = sp.GetService<HttpClient>();
                var logger = sp.GetService<ILogger<MailgunMailer>>();
                var sendValidator = sp.GetService<ISendValidator>();

                return new MailgunMailer(httpClient, options, sendValidator, logger);
            });

            return services;
        }
    }
}
