using System.Threading.Tasks;
using CoolBytes.Core.Models;

namespace CoolBytes.Core.Interfaces
{
    public interface IAuthorData
    {
        Task<bool> AuthorExists(User user);
    }
}