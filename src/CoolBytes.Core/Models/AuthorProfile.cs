using System;

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
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            About = about ?? throw new ArgumentNullException(nameof(about));
        }

        public void SetImage(Image image)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
        }
    }
}