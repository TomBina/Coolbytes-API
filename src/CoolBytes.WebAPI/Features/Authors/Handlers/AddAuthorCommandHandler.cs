using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Authors.CQ;
using CoolBytes.WebAPI.Features.Authors.ViewModels;
using MediatR;

namespace CoolBytes.WebAPI.Features.Authors.Handlers
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IAuthorValidator _authorValidator;

        public AddAuthorCommandHandler(AppDbContext context, IUserService userService, IAuthorValidator authorValidator)
        {
            _context = context;
            _userService = userService;
            _authorValidator = authorValidator;
        }

        public async Task<AuthorViewModel> Handle(AddAuthorCommand message, CancellationToken cancellationToken)
        {
            var author = await CreateAuthor(message);

            await SaveAuthor(author);

            return CreateViewModel(author);
        }

        private async Task<Author> CreateAuthor(AddAuthorCommand message)
        {
            var user = await _userService.GetOrCreateCurrentUserAsync();
            var authorProfile = await CreateAuthorProfile(message);
            var author = await Author.Create(user, authorProfile, _authorValidator);

            if (message.Experiences != null)
                await CreateExperiences(message, author);

            return author;
        }

        private async Task<AuthorProfile> CreateAuthorProfile(AddAuthorCommand message)
        {
            var socialHandles = new SocialHandles(message.LinkedIn, message.GitHub);
            var authorProfile = new AuthorProfile(message.FirstName, message.LastName, message.About);

            authorProfile.WithSocialHandles(socialHandles).WithResumeUri(message.ResumeUri);

            if (message.ImageId != null)
            {
                var image = await _context.Images.FindAsync(message.ImageId);
                authorProfile.WithImage(image);
            }

            return authorProfile;
        }

        private async Task CreateExperiences(AddAuthorCommand message, Author author)
        {
            var experiences = new List<Experience>();

            foreach (var experience in message.Experiences)
            {
                var image = await _context.Images.FindAsync(experience.ImageId);
                experiences.Add(new Experience(experience.Id, experience.Name, experience.Color, image));
            }

            author.AuthorProfile.Experiences.Update(experiences);
        }

        private async Task SaveAuthor(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        private AuthorViewModel CreateViewModel(Author author) 
            => Mapper.Map<AuthorViewModel>(author);
    }
}