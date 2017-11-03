using System;

namespace CoolBytes.Data.Models
{
    public class MailStat
    {
        public int Id { get; private set; }
        public MailProvider MailProvider { get; private set; }
        public int MailproviderId { get; private set; }
        public DateTime Date { get; private set; }
        public int Sent { get; private set; }

        public MailStat(MailProvider mailProvider, DateTime date)
        {
            MailProvider = mailProvider ?? throw new ArgumentNullException(nameof(mailProvider));
            Date = date;
        }

        public void IncrementSent() => Sent++;

        private MailStat()
        {
            
        }
    }

    public class MailProvider
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int DailyThreshold { get; private set; }

        public MailProvider(string name, int dailyThreshold)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            DailyThreshold = dailyThreshold;
        }

        private MailProvider()
        {
            
        }
    }
}
