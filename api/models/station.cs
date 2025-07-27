using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Station
    {
        public Guid StationId { get; set; }
        public required String StationName { get; set; } // The required is used to force an input when you add a new Station
        public int Order { get; set; }
    }
}