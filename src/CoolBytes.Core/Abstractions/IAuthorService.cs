using System.Threading.Tasks;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Abstractions
{
    public interface IAuthorService
    {
        Task<Author> GetAuthor();
        Task<Author> GetAuthorWithProfile();
    }
}
