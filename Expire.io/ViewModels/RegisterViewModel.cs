﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniversityProj.Helpers.Attributes;

namespace UniversityProj.ViewModels
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
        [FileExtensionsCustom("jpg,png,jpeg,gif",ErrorMessage ="Not proper format")]
        [Display(Name ="User Avatar")]
        public IFormFile Photo { get; set; }
    }
}
