using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Expire.io.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileValidationCustomAttribute : ValidationAttribute
    {
        private readonly int MaxSize = 1048576;
        private readonly string _DefaultError = "Only the following types are allowed {0}";
        private IEnumerable<string> _ValidTypes { get; set; }

        public string ErrorMessageExtension { get; set; }
        public string ErrorMessageSize { get; set; }
        public FileValidationCustomAttribute(string _errExt, string _errSize, string validTypes)
        {
            ErrorMessageExtension = _errExt;
            ErrorMessageSize = _errSize;
            _ValidTypes = validTypes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (!_ValidTypes.Any(e => file.FileName.ToLower().EndsWith(e.ToLower())))
                {
                    return new ValidationResult(ErrorMessageExtension);
                }
                if (file.Length > MaxSize)
                {
                    return new ValidationResult(ErrorMessageSize);
                }
            }
            return ValidationResult.Success;
        }
    }
}
