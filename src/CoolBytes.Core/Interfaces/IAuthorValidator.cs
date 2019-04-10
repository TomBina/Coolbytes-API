using System.Threading.Tasks;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Interfaces
{
    public interface IAuthorValidator
    {
        Task<bool> Exists(User user);
        Task<bool> Exists(IUserService userService);
    }
}