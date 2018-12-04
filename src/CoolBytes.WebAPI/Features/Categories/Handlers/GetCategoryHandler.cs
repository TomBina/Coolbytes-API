using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using CoolBytes.WebAPI.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, Result<CategoryViewModel>>
    {
        private readonly AppDbContext _context;

        public GetCategoryHandler(AppDbContext context)
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