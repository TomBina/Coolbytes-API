using CoolBytes.Core.Domain;
using CoolBytes.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Images
{
    public class DeleteImageCommandValidator : AbstractValidator<DeleteImageCommand>
    {
        private readonly AppDbContext _context;

        public DeleteImageCommandValidator(AppDbContext context)
        {
            _context = context;
        }

        public DeleteImageCommandValidator()
        {
            RuleFor(p => p.Id).CustomAsync(async (id, context, ctoken) =>
            {
                if (!await _context.Images.AnyAsync(p => p.Id == id))
                    context.AddFailure(nameof(Image.Id));
            });
        }
    }
}
