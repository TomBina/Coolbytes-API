using CoolBytes.Core.Models;
using System.Threading.Tasks;
using CoolBytes.Core.Utils;

namespace CoolBytes.Core.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Gets the current user. If no user is found in the data store, then it's creates one based on found credentials. Throws if no user is found.
        /// </summary>
        /// <returns>The current user</returns>
        Task<User> GetOrCreateCurrentUser();

        /// <summary>
        /// Tries to find the current user from the data store.
        /// </summary>
        /// <returns></returns>
        Task<Result<User>> TryGetCurrentUser();
    }
}