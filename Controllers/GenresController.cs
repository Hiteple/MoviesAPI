using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesAPI.DTOs;
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
        private readonly IMapper _mapper;

        public GenresController(ILogger<GenresController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        //[ResponseCache(Duration = 60)]
        [ServiceFilter(typeof(MyActionFilter))]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var genres = await _context.Genres.AsNoTracking().ToListAsync();
            var genresDtos = _mapper.Map<List<GenreDTO>>(genres);

            return genresDtos;
        }

        [HttpGet("{id:int}", Name = "getGenre")]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null)
            {
                //throw new ApplicationException();
                return NotFound();
            }

            var genreDto = _mapper.Map<GenreDTO>(genre);
            return genreDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreation)
        {
            // First, mapping the genre and adding it
            var genre = _mapper.Map<Genre>(genreCreation);
            _context.Add(genre);
            await _context.SaveChangesAsync();
            
            // Then, we return the Dto (which has the Id property, whereas the genreCreationDTO doesn't)
            var genreDto = _mapper.Map<GenreDTO>(genre);
            
            // Return the object created and the url for the created resource in the headers
            return new CreatedAtRouteResult("getGenre", new { genreDto.Id }, genreDto);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreation)
        {
            // First, we need to map genre to genreCreation and assign the Id manually
            var genre = _mapper.Map<Genre>(genreCreation);
            genre.Id = id;
            
            // Then, we inform that the genre has been modified and then save the changes
            _context.Entry(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _context.Genres.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            _context.Remove(new Genre() {Id = id});
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}