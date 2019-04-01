using System;

namespace CoolBytes.Services.Mailer
{
    public class EmailAddress
    {
        public string DisplayName { get; }
        public string Email { get; }

        public EmailAddress(string displayName, string email)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
}