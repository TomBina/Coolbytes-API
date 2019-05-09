using CoolBytes.Core.Abstractions;
using CoolBytes.Core.Domain;
using CoolBytes.Data;
using CoolBytes.WebAPI.Features.Authors.CQ;
using CoolBytes.WebAPI.Features.Authors.ViewModels;
using CoolBytes.WebAPI.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoolBytes.WebAPI.Features.Authors.Handlers
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _dbContext;
        private readonly HandlerContext<AuthorViewModel> _context;
        private readonly IAuthorService _authorService;

        public UpdateAuthorCommandHandler(HandlerContext<AuthorViewModel> context, IAuthorService authorService)
        {
            _dbContext = context.DbContext;
            _context = context;
            _authorService = authorService;
        }

        public async Task<AuthorViewModel> Handle(UpdateAuthorCommand message, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetAuthorWithProfile();

            await UpdateAuthorProfile(author, message);

            await SaveAuthor();

            return CreateViewModel(author);
        }

        private async Task UpdateAuthorProfile(Author author, UpdateAuthorCommand message)
        {
            var socialHandles = new SocialHandles(message.LinkedIn, message.GitHub);

            if (author.AuthorProfile.SocialHandles != null)
                _dbContext.Entry(author.AuthorProfile.SocialHandles).State = EntityState.Modified;

            author.AuthorProfile.Update(message.FirstName, message.LastName, message.About)
                                .WithSocialHandles(socialHandles)
                                .WithResumeUri(message.ResumeUri);

            if (message.ImageId != null)
                await CreateImage(author, message);

            if (message.Experiences != null)
                await CreateExperiences(message, author);
        }

        private async Task CreateImage(Author author, UpdateAuthorCommand message)
        {
            var image = await _dbContext.Images.FindAsync(message.ImageId);
            author.AuthorProfile.WithImage(image);
        }

        private async Task CreateExperiences(UpdateAuthorCommand message, Author author)
        {
            var experiences = new List<Experience>();

            foreach (var experience in message.Experiences)
            {
                var image = await _dbContext.Images.FindAsync(experience.ImageId);
                experiences.Add(new Experience(experience.Id, experience.Name, experience.Color, image));
            }

            author.AuthorProfile.Experiences.Update(experiences);
        }

        private async Task SaveAuthor()
            => await _dbContext.SaveChangesAsync();

        private AuthorViewModel CreateViewModel(Author author)
            => _context.Map(author);
    }
}