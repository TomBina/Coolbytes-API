using System;
using CoolBytes.Core.Collections;

namespace CoolBytes.Core.Domain
{
    public class AuthorProfile
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Image Image { get; private set; }
        public int? ImageId { get; private set; }
        public string About { get; private set; }
        public string ResumeUri { get; private set; }
        public SocialHandles SocialHandles { get; private set; }
        public UpdatableCollection<Experience> Experiences { get; private set; }
        public Author Author { get; private set; }
        public int AuthorId { get; private set; }

        public AuthorProfile(string firstName, string lastName, string about)
        {
            Experiences = new ExperienceCollection();
            Init(firstName, lastName, about);
        }

        private AuthorProfile() 
            => Experiences = new ExperienceCollection();

        public AuthorProfile Update(string firstName, string lastName, string about)
        {
            Init(firstName, lastName, about);
            return this;
        }

        private void Init(string firstName, string lastName, string about)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            About = about ?? throw new ArgumentNullException(nameof(about));
        }

        public AuthorProfile WithImage(Image image)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
            return this;
        }

        public AuthorProfile WithSocialHandles(SocialHandles socialHandles)
        {
            if (SocialHandles == null)
            {
                SocialHandles = socialHandles;
            }
            else
            {
                SocialHandles.Update(socialHandles.LinkedIn, socialHandles.GitHub);
            }

            return this;
        }

        public AuthorProfile WithResumeUri(string resumeUri)
        {
            ResumeUri = resumeUri;
            return this;
        }
    }
}