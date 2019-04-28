using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.WebAPI.Features.Authors.CQ;
using CoolBytes.WebAPI.Features.Authors.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Authors.Handlers
{
    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, AuthorViewModel>
    {
        private readonly HandlerContext<AuthorViewModel> _context;
        private readonly IAuthorService _authorService;

        public GetAuthorQueryHandler(HandlerContext<AuthorViewModel> context, IAuthorService authorService)
        {
            _context = context;
            _authorService = authorService;
        }

        public async Task<AuthorViewModel> Handle(GetAuthorQuery message, CancellationToken cancellationToken)
        {
            Author author;
            if (message.IncludeProfile)
            {
                author = await _authorService.GetAuthorWithProfile();
            }
            else
            {
                author = await _authorService.GetAuthor();
            }

            return CreateViewModel(author);
        }

        private AuthorViewModel CreateViewModel(Author author) => _context.Map(author);
    }
}