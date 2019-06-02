using CoolBytes.WebAPI.Features.Categories.CQ;
using FluentValidation;

namespace CoolBytes.WebAPI.Features.Categories.Validators
{
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Description).NotEmpty();
        }
    }
}
