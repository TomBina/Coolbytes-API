using System.Collections.Generic;
using CoolBytes.Core.Utils;
using MediatR;

namespace CoolBytes.WebAPI.Features.Categories.CQ
{
    public class ReSortCategoriesCommand : IRequest<Result>
    {
        public List<int> NewSortOrder { get; set; }
    }
}