using System.Collections.Generic;
using CoolBytes.Core.Utils;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class SortBlogsCommand : IRequest<Result>
    {
        public int CategoryId { get; set; }
        public List<int> NewSortOrder { get; set; }
    }
}