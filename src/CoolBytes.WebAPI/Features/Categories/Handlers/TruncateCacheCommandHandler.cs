using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.CQ;
using CoolBytes.WebAPI.Services.Caching;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class TruncateCacheCommandHandler : IRequestHandler<TruncateCacheCommand, Result>
    {
        private readonly ICacheService _cacheService;

        public TruncateCacheCommandHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(TruncateCacheCommand request, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAllAsync();

            return Result.SuccessResult();
        }
    }
}