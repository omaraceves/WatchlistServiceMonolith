using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchlistMonolith.Context;
using WatchlistMonolith.Entities;

namespace WatchlistMonolith.Services
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private MediasContext _context;

        public WatchlistRepository(MediasContext context)
        {
            _context = context;
        }

        public void AddWatchlist(Watchlist watchlistToAdd)
        {
            _context.AddAsync(watchlistToAdd);
        }

       

        public async Task<Watchlist> GetWatchlistAsync(Guid id)
        {
            var watchlist = await _context.Watchlists.Include(x => x.Medias)
                .ThenInclude(m => m.Media)
                .ThenInclude(m => m.MediaGroup)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(watchlist == null)
            {
                return null;
            }

            return watchlist;
        }

       

        public Task<IEnumerable<Watchlist>> GetWatchlistsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Watchlist>> GetWatchlistsAsync(IEnumerable<Guid> watchlistIds)
        {
            throw new NotImplementedException();
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

        public async Task<bool> AddWatchlistMediaEntryAsync(WatchlistMedia entryToAdd)
        {
            var watchList = await _context.Watchlists.FirstOrDefaultAsync(w => w.Id == entryToAdd.WatchlistId);

            if(watchList == null)
            {
                return false;
            }

            entryToAdd.Id = Guid.NewGuid();
            _context.WatchlistMedia.Add(entryToAdd);
           
            return true;
        }

        public async Task<bool> RemoveWatchlistEntryAsync(Guid watchlaterId, Guid mediaId)
        {
            var entryToRemove = await _context.WatchlistMedia.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.WatchlistId == watchlaterId);

            if (entryToRemove == null)
            {
                return false;
            }

            _context.WatchlistMedia.Remove(entryToRemove);

            return true;
        }

        public async Task<WatchlistMedia> GetWatchListMediaEntry(Guid watchlaterId, Guid mediaId)
        {
            var entry = await _context.WatchlistMedia.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.WatchlistId == watchlaterId);
            if(entry == null)
            {
                return entry;
            }

            return entry;
        }

        public Task<IEnumerable<WatchlistMedia>> GetWatchlistMediasAsync(Guid watchlistId)
        {
            throw new NotImplementedException();
        }
    }
}
