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

        public static async Task<Author> Create(User user, AuthorProfile authorProfile, IAuthorData authorData)
        {
            user.IsNotNull();
            authorProfile.IsNotNull();
            authorData.IsNotNull();

            if (await authorData.AuthorExists(user))
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