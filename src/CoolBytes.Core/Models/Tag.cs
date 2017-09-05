namespace CoolBytes.Core.Models
{
    public class Tag
    {
        public string Name { get; private set; }

        public Tag(string name)
        {
            Name = name;
        }

        private Tag()
        {
        }
    }
}