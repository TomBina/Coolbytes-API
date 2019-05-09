using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Utils;
using CoolBytes.Services.Caching;
using CoolBytes.WebAPI.Features.Categories.CQ;
using MediatR;

namespace CoolBytes.WebAPI.Features.Caching
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