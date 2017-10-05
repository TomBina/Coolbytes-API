using CoolBytes.Core.Interfaces;
using CoolBytes.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator(AppDbContext appDbContext, IUserService userService)
        {
            RuleFor(a => a.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.LastName).NotEmpty().MaximumLength(50);
            RuleFor(a => a.About).NotEmpty().MaximumLength(500);
            RuleFor(a => a).CustomAsync(async (command, context, token) =>
            {
                var user = await userService.GetUser();
                if (!await appDbContext.Authors.AnyAsync(a => a.UserId == user.Id)) 
                    context.AddFailure("No author found!!");
            });
        }
    }
}