using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityProj.Models.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public DocumentImage Image { get; set; }
        public int TypeOfDocId { get; set; }
        public TypeOfDoc TypeOfDoc { get; set; }
        public ICollection<UserDocument> UserDocuments { get; set; }
    }
}
