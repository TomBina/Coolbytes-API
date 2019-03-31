using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.CQ;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Services.Caching;

namespace CoolBytes.WebAPI.Features.Categories.Handlers
{
    public class InvalidateCacheCommandHandler : IRequestHandler<InvalidateCacheCommand, Result>
    {
        private readonly ICacheService _cacheService;

        public InvalidateCacheCommandHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(InvalidateCacheCommand request, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAllAsync();

            return Result.SuccessResult();
        }
    }
}