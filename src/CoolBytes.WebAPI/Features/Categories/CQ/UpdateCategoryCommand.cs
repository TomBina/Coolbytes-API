using CoolBytes.Core.Utils;
using MediatR;

namespace CoolBytes.WebAPI.Features.Categories.CQ
{
    public class UpdateCategoryCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}