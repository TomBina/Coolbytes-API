namespace CoolBytes.WebAPI.Features.Contact
{
    public class SendEmailCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}