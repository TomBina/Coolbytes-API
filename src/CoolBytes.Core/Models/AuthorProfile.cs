using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Models
{
    public class AuthorProfile
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Photo Photo { get; private set; }
        public int? PhotoId { get; private set; }
        public string About { get; private set; }
        public Author Author { get; set; }

        public AuthorProfile(string firstName, string lastName, string about)
        {
            firstName.IsNotNullOrWhiteSpace();
            lastName.IsNotNullOrWhiteSpace();
            about.IsNotNullOrWhiteSpace();

            FirstName = firstName;
            LastName = lastName;
            About = about;
        }

        public AuthorProfile(string firstName, string lastName, string about, Photo photo) : this(firstName, lastName, about)
        {
            photo.IsNotNull();
            Photo = photo;
        }

        private AuthorProfile()
        {
            
        }
    }
}