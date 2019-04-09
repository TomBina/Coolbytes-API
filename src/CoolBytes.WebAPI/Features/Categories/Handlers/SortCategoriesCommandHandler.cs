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
    public class SortCategoriesCommandHandler : IRequestHandler<SortCategoriesCommand, Result>
    {
        private readonly AppDbContext _context;

        public SortCategoriesCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(SortCategoriesCommand request, CancellationToken cancellationToken)
        {
            var allCategories = await Sort(request);
            await Save(allCategories);

            return Result.SuccessResult();
        }

        private async Task<List<Category>> Sort(SortCategoriesCommand request)
        {
            var allCategories = await _context.Categories.ToListAsync();
            var sorter = new Sorter<Category>();

            sorter.Sort(allCategories, request.NewSortOrder);
            return allCategories;
        }

        private async Task Save(List<Category> allCategories)
        {
            _context.AddRange(allCategories);

            await Task.CompletedTask;
        }
    }
}
