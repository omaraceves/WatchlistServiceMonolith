using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WatchlistMonolith.Context;
using WatchlistMonolith.Entities;

namespace WatchlistMonolith.Services
{
    public class MediasRepository : IMediasRepository
    {
        private MediasContext _context;

        public MediasRepository(MediasContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddMedia(Media mediaToAdd)
        {
            if(mediaToAdd == null)
            {
                throw new ArgumentNullException(nameof(mediaToAdd));
            }

            _context.Add(mediaToAdd);
        }

        public async Task<Media> GetMediaAsync(Guid id)
        {
            var result = await _context.Medias.Include(m => m.MediaGroup).
                FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }

        public async Task<IEnumerable<Media>> GetMediasAsync()
        {
            var result = await _context.Medias.Include(m => m.MediaGroup).OrderBy(m => m.MediaGroup.Name).ToListAsync();
            
            return result;
        }

        public async Task<IEnumerable<Media>> GetMediasAsync(IEnumerable<Guid> mediaIds)
        {
            var result = await _context.Medias.Where(m => mediaIds.Contains(m.Id)).Include(m => m.MediaGroup).ToListAsync();

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
    }
}
