using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Part
    {
        public Guid PartId { get; set; }
        public String? PartName { get; set; }

        // Constructor to add new parts
        public Part(String name)
        {
            PartId = Guid.NewGuid();
            PartName = name;
        }
    }
}