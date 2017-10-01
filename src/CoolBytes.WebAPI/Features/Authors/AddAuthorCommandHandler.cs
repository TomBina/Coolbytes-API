using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using System.Threading.Tasks;
using CoolBytes.Core.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class AddAuthorCommandHandler : IAsyncRequestHandler<AddAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly IAuthorValidator _authorValidator;
        private readonly IPhotoFactory _photoFactory;
        private readonly IConfiguration _configuration;

        public AddAuthorCommandHandler(AppDbContext appDbContext, IUserService userService, IAuthorValidator authorValidator, IConfiguration configuration, IPhotoFactory photoFactory)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            _authorValidator = authorValidator;
            _configuration = configuration;
            _photoFactory = photoFactory;
        }

        public async Task<AuthorViewModel> Handle(AddAuthorCommand message)
        {
            var author = await CreateAuthor(message);

            await SaveAuthor(author);

            return CreateViewModel(author);
        }

        private async Task<Author> CreateAuthor(AddAuthorCommand message)
        {
            var user = await _userService.GetUser();
            var authorProfile = new AuthorProfile(message.FirstName, message.LastName, message.About);
            var author = await Author.Create(user, authorProfile, _authorValidator);

            if (message.File == null)
                return author;

            var photo = await CreatePhoto(message.File);
            author.AuthorProfile.SetPhoto(photo);
            return author;
        }

        private async Task<Photo> CreatePhoto(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
                return await _photoFactory.Create(stream, file.FileName, file.ContentType);
        }

        private async Task SaveAuthor(Author author)
        {
            _appDbContext.Authors.Add(author);
            await _appDbContext.SaveChangesAsync();
        }

        private AuthorViewModel CreateViewModel(Author author)
        {
            var viewModel = Mapper.Map<AuthorViewModel>(author);
            viewModel.Photo?.FormatPhotoUri(_configuration);

            return viewModel;
        }
    }
}