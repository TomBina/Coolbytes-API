using System;
using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Models
{
    public class Author
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Photo Photo { get; private set; }
        public int? PhotoId { get; private set; }
        public string About { get; private set; }
        public User User { get; private set; }
        public int UserId { get; private set; }

        public Author(User user, string firstName, string lastName, string about)
        {
            user.IsNotNull();
            firstName.IsNotNullOrWhiteSpace();
            lastName.IsNotNullOrWhiteSpace();
            about.IsNotNullOrWhiteSpace();

            User = user;
            FirstName = firstName;
            LastName = lastName;
            About = about;
        }

        public Author(User user, string firstName, string lastName, string about, Photo photo)
        {
            user.IsNotNull();
            firstName.IsNotNullOrWhiteSpace();
            lastName.IsNotNullOrWhiteSpace();
            about.IsNotNullOrWhiteSpace();
            photo.IsNotNull();

            FirstName = firstName;
            LastName = lastName;
            Photo = photo;
            About = about;
        }

        private Author() { }
    }
}