using System;
using System.Threading.Tasks;
using CoolBytes.Data;
using CoolBytes.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Services.Mailer
{
    public class ThresholdValidator : ISendValidator
    {
        private readonly AppDbContext _context;

        public ThresholdValidator(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsSendingAllowed(IMailer mailer)
        {
            var mailProvider = await GetMailProvider(mailer);
            var mailStat = await GetMailStat(mailProvider);

            return await IsThresholdReached(mailStat, mailProvider);
        }

        private async Task<bool> IsThresholdReached(MailStat mailStat, MailProvider mailProvider)
        {
            if (mailStat.Sent >= mailProvider.DailyThreshold)
                return false;

            await IncrementMailStat(mailStat);

            return true;
        }

        private async Task IncrementMailStat(MailStat mailStat)
        {
            mailStat.IncrementSent();
            await _context.SaveChangesAsync();
        }

        private async Task<MailStat> GetMailStat(MailProvider mailProvider)
        {
            var mailStat = await _context.MailStats.FirstOrDefaultAsync(ms => 
                ms.MailproviderId == mailProvider.Id 
                && 
                ms.Date.Date == DateTime.Now.Date);

            return mailStat ?? CreateMailStat(mailProvider);
        }

        private MailStat CreateMailStat(MailProvider mailProvider)
        {
            var mailStat = new MailStat(mailProvider, DateTime.Now);
            _context.MailStats.Add(mailStat);

            return mailStat;
        }

        private async Task<MailProvider> GetMailProvider(IMailer mailer)
        {
            var mailerName = mailer.GetType().FullName;
            var mailProvider = await _context.MailProviders.FirstOrDefaultAsync(mp => mp.Name == mailerName);

            return mailProvider ?? CreateMailProvider(mailerName);
        }

        private MailProvider CreateMailProvider(string mailerName)
        {
            var mailProvider = new MailProvider(mailerName, 300);
            _context.MailProviders.Add(mailProvider);

            return mailProvider;
        }
    }
}