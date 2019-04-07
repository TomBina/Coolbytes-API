using System.Threading.Tasks;
using CoolBytes.Core.Domain;

namespace CoolBytes.Services
{
    public interface IAuthorSearchService
    {
        Task<Author> GetAuthorWithProfile(int id);
    }
}