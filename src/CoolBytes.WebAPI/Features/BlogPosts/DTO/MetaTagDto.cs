using AutoMapper;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.BlogPosts.DTO
{
    [AutoMap(typeof(MetaTag))]
    public class MetaTagDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
