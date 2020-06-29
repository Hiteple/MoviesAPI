using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/person")]
    public class PersonController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string _containerName = "images";

        public PersonController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonDTO>>> Get()
        {
            var people = await _context.Persons.ToListAsync();
            
            return _mapper.Map<List<PersonDTO>>(people);
        }

        [HttpGet("{id}", Name = "getPerson")]
        public async Task<ActionResult<PersonDTO>> Get(int id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            return _mapper.Map<PersonDTO>(person);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PersonCreationDTO personCreationDto)
        {
            var person = _mapper.Map<Person>(personCreationDto);

            if (personCreationDto.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personCreationDto.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(personCreationDto.Picture.FileName);
                    var contentType = personCreationDto.Picture.ContentType;
                    person.Picture = await _fileStorageService.SaveFile(content, extension, _containerName, contentType);
                }   
            }

            _context.Add(person);
            await _context.SaveChangesAsync();
            var personDto = _mapper.Map<PersonDTO>(person);
            
            return new CreatedAtRouteResult("getPerson", new { person.Id, personDto });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] PersonCreationDTO personCreationDto)
        {
            var personDb = await _context.Persons.FirstOrDefaultAsync(x => x.Id == id);

            if (personDb == null)
            {
                return NotFound();
            }

            // Update the file only if it comes in the request
            personDb = _mapper.Map(personCreationDto, personDb);
            if (personCreationDto.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personCreationDto.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(personCreationDto.Picture.FileName);
                    var contentType = personCreationDto.Picture.ContentType;
                    personDb.Picture = await _fileStorageService.EditFile(content, extension, _containerName, personDb.Picture, contentType);
                }   
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _context.Persons.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            _context.Remove(new Person() {Id = id});
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}