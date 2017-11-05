using FluentValidation;

namespace CoolBytes.WebAPI.Features.Contact
{
    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.Message).NotEmpty().MaximumLength(2000);
        }   
    }
}