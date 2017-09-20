using MediatR;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class GetAuthorQuery : IRequest<AuthorViewModel>
    {
    }
}