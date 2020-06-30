using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MoviesAPI.Validations;

namespace MoviesAPI.DTOs
{
    public class MovieCreationDTO: MoviePatchDTO
    {
        [FileSize(4)]
        [ContentType(ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }
    }
}