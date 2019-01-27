using CoolBytes.Core.Utils;
using MediatR;

namespace CoolBytes.WebAPI.Features.Categories.CQ
{
    public class InvalidateCacheCommand : IRequest<Result>
    {
    }
}