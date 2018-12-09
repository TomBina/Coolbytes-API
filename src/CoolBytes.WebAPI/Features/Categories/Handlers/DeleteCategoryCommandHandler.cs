using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly AppDbContext _context;

        public DeleteCategoryCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);

            if (category == null)
                return Result.NotFoundResult();

            if (await _context.BlogPosts.AnyAsync(b => b.CategoryId == category.Id,
                cancellationToken: cancellationToken))
                return Result.ErrorResult("Category may not have any related blog posts.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.SuccessResult();
        }
    }
}