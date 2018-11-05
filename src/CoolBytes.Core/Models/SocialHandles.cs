namespace CoolBytes.Core.Models
{
    public class SocialHandles
    {
        public string LinkedIn { get; private set; }
        public string GitHub { get; private set; }

        public SocialHandles(string linkedIn, string gitHub)
        {
            LinkedIn = linkedIn;
            GitHub = gitHub;
        }

        private SocialHandles()
        {
            
        }

        public void Update(string linkedIn, string gitHub)
        {
            LinkedIn = linkedIn;
            GitHub = gitHub;
        }
    }
}