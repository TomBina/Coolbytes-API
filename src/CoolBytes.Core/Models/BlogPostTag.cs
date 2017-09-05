namespace CoolBytes.Core.Models
{
    public class BlogPostTag
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public BlogPostTag(string name)
        {
            Name = name;
        }

        private BlogPostTag()
        {
        }
    }
}