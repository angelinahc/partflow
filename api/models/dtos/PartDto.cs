using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.dtos
{
    public class PartDto
    {
        public Guid PartId { get; set; }
        public required string PartNumber { get; set; }
        public required string PartName { get; set; }
        public required string Status { get; set; }
        public bool IsActive { get; set; }
    }
}