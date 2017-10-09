namespace CoolBytes.Core.Interfaces
{
    public interface IBlogPostContent
    {
        string Subject { get; set; }
        string ContentIntro { get; set; }
        string Content { get; set; }
    }
}