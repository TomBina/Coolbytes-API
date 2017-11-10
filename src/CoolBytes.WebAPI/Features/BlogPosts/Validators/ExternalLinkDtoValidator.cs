using CoolBytes.WebAPI.Features.BlogPosts.DTO;
using FluentValidation;
using System;

namespace CoolBytes.WebAPI.Features.BlogPosts.Validators
{
    public class ExternalLinkDtoValidator : AbstractValidator<ExternalLinkDto>
    {
        public ExternalLinkDtoValidator()
        {
            RuleFor(e => e.Name).MaximumLength(50).NotEmpty();
            RuleFor(e => e.Url).MaximumLength(255).Must(u => Uri.IsWellFormedUriString(u, UriKind.Absolute)).NotEmpty();
        }
    }
}
