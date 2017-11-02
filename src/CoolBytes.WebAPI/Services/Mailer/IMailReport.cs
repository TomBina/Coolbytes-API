namespace CoolBytes.WebAPI.Services.Mailer
{
    public interface IMailReport
    {
        string Id { get; }
        bool IsSend { get; }
    }
}