namespace CoolBytes.Services.Mailer
{
    public interface IMailReport
    {
        string Id { get; }
        bool IsSend { get; }
    }
}