using System;
using System.Threading.Tasks;
using CoolBytes.Core.Extensions;
using CoolBytes.Core.Interfaces;

namespace CoolBytes.Core.Models
{
    public class Author
    {
        public int Id { get; private set; }
        public User User { get; private set; }
        public int UserId { get; private set; }
        public AuthorProfile AuthorProfile { get; private set; }

        public static async Task<Author> Create(User user, AuthorProfile authorProfile, IAuthorValidator authorValidator)
        {
            user.IsNotNull();
            authorProfile.IsNotNull();
            authorValidator.IsNotNull();

            if (await authorValidator.Exists(user))
                throw new InvalidOperationException("Only one author can be created per user.");

            return new Author(user, authorProfile);
        }

        private Author(User user, AuthorProfile authorProfile)
        {
            User = user;
            AuthorProfile = authorProfile;
        }

        private Author()
        {
            
        }
    }
}