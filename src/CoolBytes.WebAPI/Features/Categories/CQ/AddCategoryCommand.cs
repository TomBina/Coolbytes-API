using CoolBytes.Core.Utils;
using MediatR;

namespace CoolBytes.WebAPI.Features.Categories.CQ
{
    public class AddCategoryCommand : IRequest<Result>
    {
        public string Name { get; set; }
    }
}