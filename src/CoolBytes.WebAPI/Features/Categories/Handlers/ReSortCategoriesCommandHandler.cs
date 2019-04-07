using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Categories.CQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class ReSortCategoriesCommandHandler : IRequestHandler<ReSortCategoriesCommand, Result>
    {
        private readonly AppDbContext _context;

        public ReSortCategoriesCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ReSortCategoriesCommand request, CancellationToken cancellationToken)
        {
            var allCategories = await ReSort(request);
            await Save(allCategories);

            return Result.SuccessResult();
        }

        private async Task<List<Category>> ReSort(ReSortCategoriesCommand request)
        {
            var allCategories = await _context.Categories.ToListAsync();
            var reSorter = new ReSorter<Category>();

            reSorter.Sort(allCategories, request.NewSortOrder);
            return allCategories;
        }

        private async Task Save(List<Category> allCategories)
        {
            _context.AddRange(allCategories);

            await _context.SaveChangesAsync();
        }
    }
}
