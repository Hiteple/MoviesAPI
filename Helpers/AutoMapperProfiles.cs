using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // GENRE
            // This maps the properties from Genre to GenreDTO automatically (and accepts both sideways)
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            
            // PERSON
            CreateMap<Person, PersonDTO>().ReverseMap();
            
            // Same here but I need to ignore the Picture property
            CreateMap<PersonCreationDTO, Person>()
                .ForMember(x => x.Picture, options => options.Ignore());

            CreateMap<Person, PersonPatchDTO>().ReverseMap();
            
            // Movie
            CreateMap<Movie, MovieDTO>().ReverseMap();
            
            // Same here but I need to ignore the Picture property
            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore());

            CreateMap<Movie, MoviePatchDTO>().ReverseMap();
        }
    }
}