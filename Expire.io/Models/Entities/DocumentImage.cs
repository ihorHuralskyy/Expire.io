using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.Models.Entities
{
    public class DocumentImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
