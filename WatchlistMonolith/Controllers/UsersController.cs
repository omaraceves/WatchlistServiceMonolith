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
    public class UsersController : ControllerBase
    {
        private IUsersRepository _repo;
        private IWatchlistRepository _watchlistRepo;
        private IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="repository"></param>
        public UsersController(IUsersRepository repository, IMapper mapper, IWatchlistRepository watchlistRepository)
        {
            _repo = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _watchlistRepo = watchlistRepository ?? throw new ArgumentNullException(nameof(watchlistRepository));
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>Returns a collection of Users</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userEntities = await _repo.GetUsersAsync();

            var result = _mapper.Map<IEnumerable<Model.UserModel>>(userEntities);

            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(Model.UserModel))]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var userEntity = await _repo.GetUserAsync(Guid.Parse(id));

            if(userEntity == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Model.UserModel>(userEntity);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type=typeof(Model.UserModel))]
        public async Task<IActionResult> AddUser([FromBody] Model.UserForCreation userToAdd)
        {
            var userEntity = _mapper.Map<Entities.User>(userToAdd);
            var userWatchlater = new Entities.Watchlist(userToAdd.UserName);
            userEntity.WatchLaterId = userWatchlater.Id;
            userEntity.WatchLater = userWatchlater;

            _watchlistRepo.AddWatchlist(userWatchlater);
            _repo.AddUser(userEntity);
            await _repo.SaveChangesAsync();
            var resultEntity = await _repo.GetUserAsync(userEntity.Id);
            var result = _mapper.Map<Model.UserModel>(resultEntity);


            return CreatedAtRoute("GetUser", new { id = result.Id }, result);

        }

        /// <summary>
        /// Authenticates a registered user.
        /// </summary>
        /// <param name="userCreds">User Credentials</param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Model.UserModel))]
        public async Task<IActionResult> Authenticate([FromBody]Model.UserForAuthentication userCreds)
        {
            var user = await _repo.Authenticate(userCreds.UserEmail, userCreds.Password);

            if (user == null)
                return BadRequest("Username or password is incorrect");

            var result = _mapper.Map<Model.UserModel>(user);

            // return basic user info (without password)
            return Ok(result);
        }
    }
}