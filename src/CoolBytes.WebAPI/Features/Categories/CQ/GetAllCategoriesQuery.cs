using System.Collections.Generic;
using CoolBytes.WebAPI.Features.Categories.ViewModels;
using CoolBytes.WebAPI.Utils;
using MediatR;

namespace CoolBytes.WebAPI.Features.Categories.CQ
{
    public class GetAllCategoriesQuery : IRequest<Result<IEnumerable<CategoryViewModel>>>
    {
    }
}