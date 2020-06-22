using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Entities;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController: ControllerBase
    {
        private readonly IRepository _repository;

        public GenresController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await _repository.GetAllGenres();
        }

        [HttpGet("{id:int}", Name = "getGenre")]
        public ActionResult<Genre> Get(int id)
        {
            var genre = _repository.GetGenreById(id);

            if (genre == null)
            {
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