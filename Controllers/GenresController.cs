using System.Collections.Generic;
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
        public ActionResult<List<Genre>> Get()
        {
            return _repository.GetAllGenres();
        }

        [HttpGet("{id:int}")]
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
        public ActionResult Post()
        {
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put()
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