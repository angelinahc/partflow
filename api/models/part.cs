using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Part
    {
        public Guid PartId { get; set; } = Guid.NewGuid(); // Property initializer: When you declare a new part, you already assign its default value
        public required String PartNumber { get; set; }
        public required String PartName { get; set; }
        public Status Status { get; set; } = Status.Receiving;
        public bool IsActive { get; set; } = true;
    }
}