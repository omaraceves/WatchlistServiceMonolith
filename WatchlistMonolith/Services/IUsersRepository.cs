using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchlistMonolith.Entities;

namespace WatchlistMonolith.Services
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(Guid id);

        Task<IEnumerable<User>> GetUsersAsync(IEnumerable<Guid> userIds);

        void AddUser(User userToAdd);

        Task<bool> SaveChangesAsync();

        Task<User> Authenticate(string userEmail, string password);
    }
}
