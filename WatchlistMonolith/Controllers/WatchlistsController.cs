using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchlistMonolith.Services;

namespace WatchlistMonolith.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistsController : ControllerBase
    {
        private IWatchlistRepository _watchlistRepo;
        private IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="repository"></param>
        public WatchlistsController(IMapper mapper, IWatchlistRepository watchlistRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _watchlistRepo = watchlistRepository ?? throw new ArgumentNullException(nameof(watchlistRepository));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Model.WatchlistModel))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddWatchlistEntry([FromBody] Model.WatchlistEntry entry)
        {
            //Check if the entry exists
            var watchlistEntryEntity = await _watchlistRepo.GetWatchListMediaEntry(Guid.Parse(entry.WatchlistId), Guid.Parse(entry.MediaId));

            if(watchlistEntryEntity != null)
            {
                return NoContent(); //If entry exists it means the operation was done successfully, just return No content.
            }
            else
            {
                watchlistEntryEntity = _mapper.Map<Entities.WatchlistMedia>(entry); //If entry doesn't exists, map entry to a new entity
            }

            //Add to watchlits
            await _watchlistRepo.AddWatchlistMediaEntryAsync(watchlistEntryEntity);
            var success = await _watchlistRepo.SaveChangesAsync();

            if(!success)
            {
                return BadRequest("Bad request");
            }

            //Get Watchlist with updated entry
            var updatedList = await _watchlistRepo.GetWatchlistAsync(Guid.Parse(entry.WatchlistId));
            var listResult = _mapper.Map<Model.WatchlistModel>(updatedList);

            //Return result
            return Ok(listResult);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveWatchListEntry([FromBody] Model.WatchlistEntry entry)
        {
            var success = await _watchlistRepo.RemoveWatchlistEntryAsync(Guid.Parse(entry.WatchlistId), Guid.Parse(entry.MediaId));

            if(!success)
            {
                return NotFound();
            }

            await _watchlistRepo.SaveChangesAsync();

            return NoContent();
        }
    }
}