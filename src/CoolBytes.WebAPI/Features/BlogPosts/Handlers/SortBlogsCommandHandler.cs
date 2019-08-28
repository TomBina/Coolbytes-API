using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Domain;
using CoolBytes.Core.Utils;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.BlogPosts.CQ;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.BlogPosts.Handlers
{
    public class SortBlogsCommandHandler : IRequestHandler<SortBlogsCommand, Result>
    {
        private readonly AppDbContext _dbContext;

        public SortBlogsCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(SortBlogsCommand request, CancellationToken cancellationToken)
        {
            var allBlogs = await _dbContext.BlogPosts.Where(b => b.CategoryId == request.CategoryId).ToListAsync(cancellationToken: cancellationToken);

            var sorter = new Sorter<BlogPost>();
            sorter.Sort(allBlogs, request.NewSortOrder);

            _dbContext.BlogPosts.UpdateRange(allBlogs);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.SuccessResult();
        }
    }
}