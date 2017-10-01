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
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class UpdateAuthorCommandHandler : IAsyncRequestHandler<UpdateAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IPhotoFactory _photoFactory;
        private bool _isPhotoCreated;

        public UpdateAuthorCommandHandler(AppDbContext appDbContext, IUserService userService, IConfiguration configuration, IPhotoFactory photoFactory)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            _configuration = configuration;
            _photoFactory = photoFactory;
        }

        public async Task<AuthorViewModel> Handle(UpdateAuthorCommand message)
        {
            var author = await GetAuthor();

            await UpdateAuthor(author, message);
            await SaveAuthor(author);

            return CreateViewModel(author);
        }

        private async Task<Author> GetAuthor()
        {
            var user = await _userService.GetUser();
            var author = await _appDbContext.Authors.Include(a => a.AuthorProfile).FirstOrDefaultAsync(a => a.UserId == user.Id);
            return author;
        }

        private async Task UpdateAuthor(Author author, UpdateAuthorCommand message)
        {
            author.AuthorProfile.Update(message.FirstName, message.LastName, message.About);

            if (message.File == null)
                return;

            var photo = await CreatePhoto(message.File);
            author.AuthorProfile.SetPhoto(photo);
        }

        private async Task<Photo> CreatePhoto(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var photo = await _photoFactory.Create(stream, file.FileName, file.ContentType);
                _isPhotoCreated = true;
                return photo;
            }
        }

        private async Task SaveAuthor(Author author)
        {
            if (_isPhotoCreated)
            {
                try
                {
                    _appDbContext.Authors.Update(author);
                    await _appDbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    File.Delete(author.AuthorProfile.Photo.Path);
                    throw;
                }
            }
            else
            {
                await _appDbContext.SaveChangesAsync();
            }
        }

        private AuthorViewModel CreateViewModel(Author author)
        {
            var viewModel = Mapper.Map<AuthorViewModel>(author);
            viewModel.Photo?.FormatPhotoUri(_configuration);
            return viewModel;
        }
    }
}