using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Collections;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _context;
        private readonly IAuthorService _authorService;

        public UpdateAuthorCommandHandler(AppDbContext context, IAuthorService authorService)
        {
            _context = context;
            _authorService = authorService;
        }

        public async Task<AuthorViewModel> Handle(UpdateAuthorCommand message, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetAuthorWithProfile();

            await UpdateAuthorProfile(author, message);

            await SaveAuthor(author);

            return CreateViewModel(author);
        }

        private async Task UpdateAuthorProfile(Author author, UpdateAuthorCommand message)
        {
            var socialHandles = new SocialHandles(message.LinkedIn, message.GitHub);

            if (author.AuthorProfile.SocialHandles != null)
                _context.Entry(author.AuthorProfile.SocialHandles).State = EntityState.Modified;

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
            var image = await _context.Images.FindAsync(message.ImageId);
            author.AuthorProfile.WithImage(image);
        }

        private async Task CreateExperiences(UpdateAuthorCommand message, Author author)
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
            => await _context.SaveChangesAsync();

        private AuthorViewModel CreateViewModel(Author author)
            => Mapper.Map<AuthorViewModel>(author);
    }
}