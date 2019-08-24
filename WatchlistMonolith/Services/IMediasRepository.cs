using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchlistMonolith.Entities;

namespace WatchlistMonolith.Services
{
    public interface IMediasRepository
    {
        Task<IEnumerable<Media>> GetMediasAsync();

        Task<Media> GetMediaAsync(Guid id);

        Task<IEnumerable<Media>> GetMediasAsync(IEnumerable<Guid> mediaIds);

        void AddMedia(Media mediaToAdd);

        Task<bool> SaveChangesAsync();
    }
}
