using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Services.Mailer
{
    public interface ISendValidator
    {
        Task<bool> IsSendingAllowed(IMailer mailer);
    }
}