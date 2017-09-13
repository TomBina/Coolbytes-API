using MediatR;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class AddAuthorCommand : IRequest<AuthorViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string About { get; set; }
    }
}