using System;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.Services.Mailer
{
    public class MailgunMailerOptionsFactory
    {
        private readonly IConfiguration _configuration;

        public MailgunMailerOptionsFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MailgunMailerOptions Create()
        {
            var server = new Uri(_configuration["Mailgun:Server"]);
            var userName = _configuration["mailgunuser"];
            var password = _configuration["mailgunkey"];
            var domain = _configuration["Mailgun:Domain"];
            var credentials = new MailgunMailerCredentials(userName, password);
            var mailgunMailerOptions = new MailgunMailerOptions(server, credentials, domain);

            return mailgunMailerOptions;
        }
    }
}
