using System;

namespace CoolBytes.Services.Mailer
{
    public class MailReport : IMailReport
    {
        public string Id { get; }
        public bool IsSend { get; }

        public MailReport(bool isSend)
        {
            IsSend = isSend;
        }

        public MailReport(string id, bool isSend)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            IsSend = isSend;
        }
    }
}