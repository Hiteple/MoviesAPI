using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace MoviesAPI.Validations
{
    public class ContentType: ValidationAttribute
    {
        private readonly string[] _validContentTypes;
        private readonly string[] _imageContentTypes = {"image/jpeg", "image/png", "image/gif"};

        public ContentType(string[] validContentTypes)
        {
            _validContentTypes = validContentTypes;
        }

        public ContentType(ContentTypeGroup contentTypeGroup)
        {
            switch (contentTypeGroup)
            {
                case ContentTypeGroup.Image:
                    _validContentTypes = _imageContentTypes;
                    break;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (!_validContentTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"Content-Type should be one of the following: {string.Join(", ", _validContentTypes)}");
            }
            
            return ValidationResult.Success;
        }
    }

    public enum ContentTypeGroup
    {
        Image
    }
}