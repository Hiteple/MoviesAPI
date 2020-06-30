using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MoviesAPI.Validations;

namespace MoviesAPI.DTOs
{
    public class PersonCreationDTO: PersonPatchDTO
    {

        [FileSize(4)]
        [ContentType(ContentTypeGroup.Image)]
        public IFormFile Picture { get; set; }
    }
}