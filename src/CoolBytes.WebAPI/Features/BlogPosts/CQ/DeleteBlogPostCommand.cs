using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts.CQ
{
    public class DeleteBlogPostCommand : IRequest
    {
        public int Id { get; set; }
    }
}
