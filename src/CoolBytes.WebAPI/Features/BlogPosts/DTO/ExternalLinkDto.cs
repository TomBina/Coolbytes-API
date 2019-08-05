using AutoMapper;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.BlogPosts.DTO
{
    [AutoMap(typeof(ExternalLink))]
    public class ExternalLinkDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}