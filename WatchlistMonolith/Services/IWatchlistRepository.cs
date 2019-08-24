using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchlistMonolith.Entities;

namespace WatchlistMonolith.Services
{
    public interface IWatchlistRepository
    {
        Task<IEnumerable<Watchlist>> GetWatchlistsAsync();

        Task<Watchlist> GetWatchlistAsync(Guid id);

        Task<IEnumerable<Watchlist>> GetWatchlistsAsync(IEnumerable<Guid> watchlistIds);

        void AddWatchlist(Watchlist watchlistToAdd);

        Task<bool> AddWatchlistMediaEntryAsync(WatchlistMedia entryToAdd);

        Task<bool> RemoveWatchlistEntryAsync(Guid watchlaterId, Guid mediaId);
        Task<WatchlistMedia> GetWatchListMediaEntry(Guid watchlaterId, Guid mediaId);

        Task<IEnumerable<WatchlistMedia>> GetWatchlistMediasAsync(Guid watchlistId);
       

        Task<bool> SaveChangesAsync();
    }
}
