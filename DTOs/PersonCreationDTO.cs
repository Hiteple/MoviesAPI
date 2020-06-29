using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MoviesAPI.Validations;

namespace MoviesAPI.DTOs
{
    public class PersonCreationDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        //[FileSize(4)]
        [ContentType(ContentTypeGroup.Image)]
        public IFormFile Picture { get; set; }
    }
}