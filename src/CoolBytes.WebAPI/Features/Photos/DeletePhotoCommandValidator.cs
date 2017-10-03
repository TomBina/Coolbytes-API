using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Photos
{
    public class DeletePhotoCommandValidator : AbstractValidator<DeletePhotoCommand>
    {
        private readonly AppDbContext _context;

        public DeletePhotoCommandValidator(AppDbContext context)
        {
            _context = context;
        }

        public DeletePhotoCommandValidator()
        {
            RuleFor(p => p.Id).CustomAsync(async (id, context, ctoken) =>
            {
                if (!await _context.Photos.AnyAsync(p => p.Id == id))
                    context.AddFailure(nameof(Photo.Id));
            });
        }
    }
}
