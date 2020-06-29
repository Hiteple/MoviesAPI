using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/person")]
    public class PersonController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PersonController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
        public async Task<ActionResult> Post([FromBody] PersonCreationDTO personCreationDto)
        {
            var person = _mapper.Map<Person>(personCreationDto);
            _context.Add(person);
            await _context.SaveChangesAsync();

            var personDto = _mapper.Map<PersonDTO>(person);
            
            return new CreatedAtRouteResult("getPerson", new { person.Id, personDto });
        }
    }
}