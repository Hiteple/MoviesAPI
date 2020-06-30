using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string _containerName = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get()
        {
            var movies = await _context.Movies.ToListAsync();
            
            // We pass the movies list to the DTO
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null)
            {
                return NotFound(); 
            }
            
            return _mapper.Map<MovieDTO>(movie);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreationDto)
        {
            var movie = _mapper.Map<Movie>(movieCreationDto);
            
            if (movieCreationDto.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDto.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDto.Poster.FileName);
                    var contentType = movieCreationDto.Poster.ContentType;
                    movie.Poster = await _fileStorageService.SaveFile(content, extension, _containerName, contentType);
                }   
            }

            AnnotateActorsOrder(movie);

            _context.Add(movie);
            await _context.SaveChangesAsync();
            var movieDto = _mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, movieDto);
        }

        private static void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDto)
        {
            var movieDb = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movieDb == null)
            {
                return NotFound();
            }
            
            // Update the file only if it comes in the request
            movieDb = _mapper.Map(movieCreationDto, movieDb);
            if (movieCreationDto.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDto.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDto.Poster.FileName);
                    var contentType = movieCreationDto.Poster.ContentType;
                    movieDb.Poster = await _fileStorageService.EditFile(content, extension, _containerName, movieDb.Poster, contentType);
                }
            }

            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"deleted from MoviesActors where movieId = {movieDb.Id}; deleted from MoviesGenres where movieId = {movieDb.Id}");
            
            AnnotateActorsOrder(movieDb);
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entityFromDb = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (entityFromDb == null)
            {
                return NotFound();
            }

            // Map the entity from the DB to the PersonPatchDTO
            var entityDto = _mapper.Map<MoviePatchDTO>(entityFromDb);
            
            patchDocument.ApplyTo(entityDto, ModelState);

            // Try to validate that the model has not business logic problems
            var isValid = TryValidateModel(entityDto);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            // Now, map to the entity to entityDB
            _mapper.Map(entityDto, entityFromDb);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _context.Movies.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            _context.Remove(new Movie() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}