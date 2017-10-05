using CoolBytes.Core.Models;
using System.Threading.Tasks;

namespace CoolBytes.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser();
    }
}