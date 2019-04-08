using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Expire.io.Helpers.Attributes
{
    public class ImageValidationAttribute:ValidationAttribute
    {
        public ImageValidationAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                try
                {
                    var img = Image.Load(file.OpenReadStream());
                }
                catch
                {
                    return new ValidationResult("Invalid type");
                }
            }
            return ValidationResult.Success;
        }
    }
}
