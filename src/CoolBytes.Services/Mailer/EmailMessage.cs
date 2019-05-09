using System;

namespace CoolBytes.Services.Mailer
{
    public class EmailMessage
    {
        public EmailAddress From { get; }
        public EmailAddress To { get; }
        public string Subject { get; }
        public string Body { get; }

        public EmailMessage(EmailAddress from, EmailAddress to, string subject, string body)
        {
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            Body = body ?? throw new ArgumentNullException(nameof(body));
        }
    }
}