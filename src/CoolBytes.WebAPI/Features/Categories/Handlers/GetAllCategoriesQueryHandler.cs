using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<CategoryViewModel>>>
    {
        private readonly HandlerContext<IEnumerable<CategoryViewModel>> _context;
        private readonly AppDbContext _dbContext;

        public GetAllCategoriesQueryHandler(HandlerContext<IEnumerable<CategoryViewModel>> context)
        {
            _context = context;
            _dbContext = context.DbContext;
        }

        public async Task<Result<IEnumerable<CategoryViewModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _dbContext.Categories.OrderBy(c => c.SortOrder).ToListAsync(cancellationToken: cancellationToken);

            if (!categories.Any())
                return Result<IEnumerable<CategoryViewModel>>.NotFoundResult();

            return _context.Map(categories).ToSuccessResult();
        }
    }
}