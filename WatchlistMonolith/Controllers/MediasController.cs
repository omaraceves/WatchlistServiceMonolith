using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchlistMonolith.Services;
using AutoMapper;

namespace WatchlistMonolith.Controllers
{
    /// <summary>
    /// Medias Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MediasController : ControllerBase
    {
        private IMediasRepository _repo;
        private IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="repository"></param>
        public  MediasController(IMediasRepository repository, IMapper mapper)
        {
            _repo = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all medias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Entities.Media>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMedias()
        {
            var mediaEntities = await _repo.GetMediasAsync();
            var result = _mapper.Map<IEnumerable<Model.MediaModel>>(mediaEntities);

            return Ok(result);
        }


        /// <summary>
        /// Gets a media
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}", Name = "GetMedia")]
        [ProducesResponseType(typeof(Model.MediaModel),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMedia([FromRoute] string id)
        {
            var mediaEntity = await _repo.GetMediaAsync(Guid.Parse(id));

            if(mediaEntity == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Model.MediaModel>(mediaEntity);

            return Ok(result);
        }

        /// <summary>
        /// Adds a media
        /// </summary>
        /// <param name="mediaToAdd"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Model.MediaModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddMedia([FromBody] Model.MediaForCreation mediaToAdd)
        {
            var mediaEntityToAdd = _mapper.Map<Entities.Media>(mediaToAdd);

            _repo.AddMedia(mediaEntityToAdd);
            await _repo.SaveChangesAsync();
            var resultEntity = await _repo.GetMediaAsync(mediaEntityToAdd.Id);

            var result = _mapper.Map<Model.MediaModel>(resultEntity);


            return CreatedAtRoute("GetMedia", new { id = result.Id }, result);
        }
    }
}