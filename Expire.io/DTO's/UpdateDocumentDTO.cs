using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Expire.io.DTO_s
{
    public class UpdateDocumentDTO
    {
        public int Id { get; set; }
        public DateTime DateOfExpire { get; set; }
        public string date { get; set; }
        public IFormFile Photo { get; set; }
    }
}
