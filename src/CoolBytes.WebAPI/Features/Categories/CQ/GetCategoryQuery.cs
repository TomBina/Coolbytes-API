using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Categories.CQ
{
    public class GetCategoryQuery : IRequest<Result<CategoryViewModel>>
    {
        public int Id { get; set; }
    }
}