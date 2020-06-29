using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public GenresController(ILogger<GenresController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        //[ResponseCache(Duration = 60)]
        [ServiceFilter(typeof(MyActionFilter))]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            
            var genres = await _context.Genres.AsNoTracking().ToListAsync();
            return genres;
        }

        [HttpGet("{id:int}", Name = "getGenre")]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                //throw new ApplicationException();
                return NotFound();
            }
            return genre;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Genre genre)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
            
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