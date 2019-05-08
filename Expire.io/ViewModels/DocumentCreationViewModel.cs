using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace Expire.io.ViewModels
{
    public class DocumentCreationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public string TypeOfDocId { get; set; }
    }
}
