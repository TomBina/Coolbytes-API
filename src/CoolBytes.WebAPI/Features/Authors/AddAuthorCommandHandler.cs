using System.IO;
using AutoMapper;
using CoolBytes.Core.Interfaces;
using CoolBytes.Core.Models;
using CoolBytes.Data;
using MediatR;
using System.Threading.Tasks;
using CoolBytes.Core.Factories;
using Microsoft.AspNetCore.Http;

namespace CoolBytes.WebAPI.Features.Authors
{
    public class AddAuthorCommandHandler : IAsyncRequestHandler<AddAuthorCommand, AuthorViewModel>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUserService _userService;
        private readonly IAuthorValidator _authorValidator;
        private readonly IImageFactory _imageFactory;

        public AddAuthorCommandHandler(AppDbContext appDbContext, IUserService userService, IAuthorValidator authorValidator, IImageFactory imageFactory)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            _authorValidator = authorValidator;
            _imageFactory = imageFactory;
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

            var image = await CreateImage(message.File);
            author.AuthorProfile.SetImage(image);
            return author;
        }

        private async Task<Image> CreateImage(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
                return await _imageFactory.Create(stream, file.FileName, file.ContentType);
        }

        private async Task SaveAuthor(Author author)
        {
            _appDbContext.Authors.Add(author);

            if (author.AuthorProfile.Image != null)
                await _appDbContext.SaveChangesAsync(() => File.Delete(author.AuthorProfile.Image.Path));
            else
                await _appDbContext.SaveChangesAsync();
        }

        private AuthorViewModel CreateViewModel(Author author) => Mapper.Map<AuthorViewModel>(author);
    }
}