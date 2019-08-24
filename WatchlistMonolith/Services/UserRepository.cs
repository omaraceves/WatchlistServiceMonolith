using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchlistMonolith.Context;
using WatchlistMonolith.Entities;

namespace WatchlistMonolith.Services
{
    public class UserRepository : IUsersRepository
    {
        private MediasContext _context;

        public UserRepository(MediasContext context)
        {
            _context = context;
        }

        public void AddUser(User userToAdd)
        {
            if (userToAdd == null)
            {
                throw new ArgumentNullException(nameof(userToAdd));
            }

            _context.Add(userToAdd);
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var result = await _context.Users.Include(u => u.WatchLater)
                .ThenInclude(u => u.Medias)
                .FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.Include(x => x.WatchLater).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(IEnumerable<Guid> userIds)
        {
            var result = await _context.Users.Where(m => userIds.Contains(m.Id))
                .Include(u => u.WatchLater)
                .ToListAsync();

            return result;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync() > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> Authenticate(string userEmail, string password)
        {
            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users
                .Include(x=> x.WatchLater)
                .ThenInclude(x => x.Medias)
                .ThenInclude(x => x.Media)
                .ThenInclude(x => x.MediaGroup)
                .SingleOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
                return null;

            // check if password is correct
            if (password.CompareTo(user.Password) == -1)
                return null;

            // authentication successful
            return user;
        }
    }
}
