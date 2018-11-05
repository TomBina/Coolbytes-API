using CoolBytes.Core.Models;
using System.Threading.Tasks;

namespace CoolBytes.Core.Interfaces
{
    public interface IAuthorService
    {
        Task<Author> GetAuthor();
        Task<Author> GetAuthorWithProfile();
    }
}
