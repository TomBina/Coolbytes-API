using System;

namespace CoolBytes.Services.Mailer
{
    public class MailgunMailerCredentials
    {
        public string UserName { get; }
        public string Password { get; }

        public MailgunMailerCredentials(string userName, string password)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}