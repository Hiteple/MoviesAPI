using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MoviesAPI.Validations
{
    public class FileSize: ValidationAttribute
    {
        private readonly int _maxFileInMbs;

        public FileSize(int maxFileInMbs)
        {
            _maxFileInMbs = maxFileInMbs;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > _maxFileInMbs * 1024 * 1024)
            {
                return new ValidationResult($"File size cannot be large than {_maxFileInMbs} megabytes");
            }
            return ValidationResult.Success;
        }
    }
}