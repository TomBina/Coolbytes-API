using CoolBytes.Core.Models;
using System.Threading.Tasks;
using CoolBytes.Core.Utils;

namespace CoolBytes.Core.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Gets the current user. If no user is found in the data store, then it's creates one based on found credentials. Throws if no user credentials are found.
        /// </summary>
        /// <returns>The current user</returns>
        Task<User> GetOrCreateCurrentUserAsync();

        /// <summary>
        /// Tries to get the current user from the data store.
        /// </summary>
        /// <returns></returns>
        Task<Result<User>> TryGetCurrentUserAsync();
    }
}