using AutoMapper;
using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Result<CategoryViewModel>>
    {
        private readonly AppDbContext _context;

        public GetCategoryQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CategoryViewModel>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var foundCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken: cancellationToken);

            if (foundCategory == null)
                return Result<CategoryViewModel>.NotFoundResult();

            var viewModel = Mapper.Map<CategoryViewModel>(foundCategory);
            return viewModel.ToSuccessResult();
        }
    }
}