using System.Threading.Tasks;
using CoolBytes.Core.Models;

namespace CoolBytes.WebAPI.Services
{
    public interface IUserService
    {
        Task<User> GetUser();
    }
}