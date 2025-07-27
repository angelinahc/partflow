using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Station
    {
        public Guid StationId { get; set; }
        public String? StationName { get; set; }

        public Station(String name)
        {
            StationId = Guid.NewGuid();
            StationName = name;
        }
    }
}