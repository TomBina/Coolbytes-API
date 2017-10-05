using CoolBytes.Core.Extensions;

namespace CoolBytes.Core.Models
{
    public class AuthorProfile
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Image Image { get; private set; }
        public int? ImageId { get; private set; }
        public string About { get; private set; }
        public Author Author { get; set; }

        public AuthorProfile(string firstName, string lastName, string about) => Init(firstName, lastName, about);

        private AuthorProfile()
        {
            
        }

        public void Update(string firstName, string lastName, string about) => Init(firstName, lastName, about);

        private void Init(string firstName, string lastName, string about)
        {
            firstName.IsNotNullOrWhiteSpace();
            lastName.IsNotNullOrWhiteSpace();
            about.IsNotNullOrWhiteSpace();

            FirstName = firstName;
            LastName = lastName;
            About = about;
        }

        public void SetImage(Image image)
        {
            image.IsNotNull();

            Image = image;
        }
    }
}