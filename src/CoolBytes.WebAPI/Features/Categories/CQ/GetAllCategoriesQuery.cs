using CoolBytes.Core.Utils;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace CoolBytes.WebAPI.Features.Categories.CQ
{
    public class GetAllCategoriesQuery : IRequest<Result<IEnumerable<CategoryViewModel>>>
    {
    }
}