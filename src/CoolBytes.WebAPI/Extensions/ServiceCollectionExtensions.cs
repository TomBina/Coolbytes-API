using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.WebAPI.Services.Mailer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoolBytes.WebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMailgun(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMailer>(sp =>
            {
                var server = new Uri(configuration["Mailgun:Server"]);
                var credentials = new MailgunMailerCredentials(configuration["Mailgun:UserName"], configuration["Mailgun:Key"]);
                var domain = configuration["Mailgun:Domain"];

                var options = new MailgunMailerOptions(server, credentials, domain);
                var httpClient = sp.GetService<HttpClient>();
                var logger = sp.GetService<ILogger<MailgunMailer>>();

                return new MailgunMailer(httpClient, options, logger);
            });

            return services;
        }
    }
}
