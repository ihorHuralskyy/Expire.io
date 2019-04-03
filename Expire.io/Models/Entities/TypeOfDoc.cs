using System.Collections.Generic;

namespace Expire.io.Models.Entities
{
    public class TypeOfDoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
