using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class UpdateAuthorCommandHandler : IAsyncRequestHandler<UpdateAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IAuthorService _authorService;
        private readonly IImageFactory _imageFactory;
        private bool _isImageUpdated;

        public UpdateAuthorCommandHandler(AppDbContext appDbContext, IAuthorService authorService, IImageFactory imageFactory)
        {
            _appDbContext = appDbContext;
            _authorService = authorService;
            _imageFactory = imageFactory;
        }

        public async Task<AuthorViewModel> Handle(UpdateAuthorCommand message)
        {
            var author = await _authorService.GetAuthorWithProfile();

            await UpdateAuthor(author, message);
            await SaveAuthor(author);

            return CreateViewModel(author);
        }

        private async Task UpdateAuthor(Author author, UpdateAuthorCommand message)
        {
            author.AuthorProfile.Update(message.FirstName, message.LastName, message.About);

            if (message.File == null)
                return;

            var image = await CreateImage(message.File);
            author.AuthorProfile.SetImage(image);
        }

        private async Task<Image> CreateImage(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var image = await _imageFactory.Create(stream, file.FileName, file.ContentType);
                _isImageUpdated = true;
                return image;
            }
        }

        private async Task SaveAuthor(Author author)
        {
            if (_isImageUpdated)
                await _appDbContext.SaveChangesAsync(() => File.Delete(author.AuthorProfile.Image.Path));
            else
                await _appDbContext.SaveChangesAsync();
        }

        private AuthorViewModel CreateViewModel(Author author) => Mapper.Map<AuthorViewModel>(author);
    }
}