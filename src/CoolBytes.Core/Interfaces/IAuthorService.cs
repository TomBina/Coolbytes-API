using System.Threading.Tasks;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> GetAuthor();
        Task<Author> GetAuthorWithProfile();
    }
}
