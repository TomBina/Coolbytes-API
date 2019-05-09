using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Result<CategoryViewModel>>
    {
        private readonly HandlerContext<CategoryViewModel> _context;
        private readonly AppDbContext _dbContext;

        public GetCategoryQueryHandler(HandlerContext<CategoryViewModel> context)
        {
            _context = context;
            _dbContext = context.DbContext;
        }

        public async Task<Result<CategoryViewModel>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var foundCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);

            if (foundCategory == null)
                return Result<CategoryViewModel>.NotFoundResult();

            var viewModel = _context.Map(foundCategory);
            return viewModel.ToSuccessResult();
        }
    }
}