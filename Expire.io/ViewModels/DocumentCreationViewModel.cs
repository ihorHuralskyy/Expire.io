using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using SixLabors.ImageSharp;

namespace Expire.io.ViewModels
{
    public class DocumentCreationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public string TypeOfDocName { get; set; }
        public IFormFile Photo { get; set; }
        public string datetime { get; set; }
    }
}
