using System.Collections.Generic;
using System.Linq;
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
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<CategoryViewModel>>>
    {
        private readonly AppDbContext _context;

        public GetAllCategoriesHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<CategoryViewModel>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories.ToListAsync(cancellationToken: cancellationToken);

            if (!categories.Any())
                return Result<IEnumerable<CategoryViewModel>>.NotFoundResult();

            return Mapper.Map<IEnumerable<CategoryViewModel>>(categories).ToSuccessResult();
        }
    }
}