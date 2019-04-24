using System.Threading.Tasks;
using CoolBytes.Core.Domain;

namespace CoolBytes.Core.Abstractions
{
    public interface IAuthorValidator
    {
        Task<bool> Exists(User user);
        Task<bool> Exists(IUserService userService);
    }
}