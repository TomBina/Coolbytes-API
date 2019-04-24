using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, AuthorViewModel>
    {
        private readonly IAuthorService _authorService;

        public GetAuthorQueryHandler(IAuthorService authorService)
        {
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

        private AuthorViewModel CreateViewModel(Author author) => Mapper.Map<AuthorViewModel>(author);
    }
}