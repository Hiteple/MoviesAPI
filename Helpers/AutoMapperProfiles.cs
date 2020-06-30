using System.Collections.Generic;
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
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MoviesActors, options => options.MapFrom(MapMoviesActors));
            
            CreateMap<Movie, MoviePatchDTO>().ReverseMap();
        }

        private List<MoviesGenres> MapMoviesGenres(MovieCreationDTO movieCreationDto, Movie movie)
        {
            var result = new List<MoviesGenres>();
            foreach (var id in movieCreationDto.GenresIds)
            {
                result.Add(new MoviesGenres() { GenreId = id });
            }

            return result;
        }
        
        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDto, Movie movie)
        {
            var result = new List<MoviesActors>();
            foreach (var actor in movieCreationDto.Actors)
            {
                result.Add(new MoviesActors() { PersonId = actor.PersonId, Character = actor.Character});
            }

            return result;
        }
    }
}