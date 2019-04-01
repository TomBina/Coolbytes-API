using System.Threading.Tasks;
using CoolBytes.Core.Models;

namespace CoolBytes.Services
{
    public interface IAuthorSearchService
    {
        Task<Author> GetAuthorWithProfile(int id);
    }
}