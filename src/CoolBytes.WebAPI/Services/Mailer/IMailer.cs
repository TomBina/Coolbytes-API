using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services.Mailer
{
    public interface IMailer
    {
        Task<IMailReport> Send(EmailMessage message);
    }
}