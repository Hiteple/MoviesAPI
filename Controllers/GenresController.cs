using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoviesAPI.Entities;
using MoviesAPI.Filters;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenresController: ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public GenresController(IRepository repository, ILogger<GenresController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        //[ResponseCache(Duration = 60)]
        [ServiceFilter(typeof(MyActionFilter))]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            
            var genres = await _repository.GetAllGenres();
            _logger.LogInformation("Genres obtained!");
            return genres;
        }

        [HttpGet("{id:int}", Name = "getGenre")]
        public ActionResult<Genre> Get(int id)
        {
            var genre = _repository.GetGenreById(id);

            if (genre == null)
            {
                _logger.LogWarning($"Genre with id {id} not found");
                //throw new ApplicationException();
                return NotFound();
            }
            return genre;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genre genre)
        {
            _repository.AddGenre(genre);
            
            // Return the object created and the url for the created resource in the headers
            return new CreatedAtRouteResult("getGenre", new { genre.Id }, genre);
        }
        
        [HttpPut]
        public ActionResult Put([FromBody] Genre genre)
        {
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }
    }
}