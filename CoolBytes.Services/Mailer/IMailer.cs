using System.Threading.Tasks;

namespace CoolBytes.Services.Mailer
{
    public interface IMailer
    {
        Task<IMailReport> Send(EmailMessage message);
    }
}