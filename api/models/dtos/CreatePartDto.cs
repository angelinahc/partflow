using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.controllers.dtos
{
    public class CreatePartDto
    {
        public required string PartNumber { get; set; }
        public required string PartName { get; set; }
    }
}