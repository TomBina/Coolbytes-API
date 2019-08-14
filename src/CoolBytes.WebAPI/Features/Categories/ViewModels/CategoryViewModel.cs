using AutoMapper;
using CoolBytes.Core.Domain;

namespace CoolBytes.WebAPI.Features.Categories.ViewModels
{
    [AutoMap((typeof(Category)))]
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public bool IsCourse { get; set; }
    }
}