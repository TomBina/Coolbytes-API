using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediatR;

namespace CoolBytes.WebAPI.Features.BlogPosts
{
    public class DeleteBlogPostCommand : IRequest
    {
        public int Id { get; set; }
    }
}
