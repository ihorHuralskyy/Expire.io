﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expire.io.DTOs
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public string TypeOfDocName { get; set; }
        public string Image { get; set; }
    }
}
