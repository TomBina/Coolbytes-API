using System;

namespace CoolBytes.WebAPI.Services.Mailer
{
    public class MailgunMailerOptions
    {
        public Uri Server { get; }
        public MailgunMailerCredentials Credentials { get; }
        public string Domain { get; }

        public MailgunMailerOptions(Uri server, MailgunMailerCredentials credentials, string domain)
        {
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
            Domain = domain ?? throw new ArgumentNullException(nameof(domain));
        }
    }
}