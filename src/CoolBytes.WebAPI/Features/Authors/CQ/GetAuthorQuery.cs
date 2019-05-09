using CoolBytes.WebAPI.Features.Authors.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Authors.CQ
{
    public class GetAuthorQuery : IRequest<AuthorViewModel>
    {
        public bool IncludeProfile { get; set; }
    }
}