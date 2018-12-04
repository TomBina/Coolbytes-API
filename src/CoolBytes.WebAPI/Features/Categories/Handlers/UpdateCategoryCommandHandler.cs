﻿using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
    {
        private readonly AppDbContext _context;

        public UpdateCategoryCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);

            if (category == null)
                return Result.NotFoundResult();

            category.UpdateName(request.Name);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.SuccessResult();
        }
    }
}