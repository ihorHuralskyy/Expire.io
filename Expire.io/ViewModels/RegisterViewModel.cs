using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Expire.io.Helpers.Attributes;

namespace Expire.io.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="First Name")]
        public string FName { get; set; }
        [Required]
        [Display(Name ="Last Name")]
        public string LName { get; set; }
        [Required]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email not in proper format")]
        public string Email { get; set; }
        [Display(Name ="Phone number")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Wron input")]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Wrong password")]
        public string ConfirmPassword { get; set; }
        [ImageValidation]
        [FileValidationCustom("Invalid extension","Invalid type","jpeg,jpg,png")]
        [Display(Name ="User Avatar")]
        public IFormFile Photo { get; set; }
    }
}
