using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Flow
    {
        public Guid FlowId { get; set; }
        public Guid PartId { get; set; }
        public Guid StationId { get; set; }
        public DateTime Date { get; set; }
        public int Origin { get; set; }
        public int Destination { get; set; }
        public String? Collaborator { get; set; }

        public Flow(Guid part, Guid station, int origin, int destination, String collaborator)
        {
            FlowId = Guid.NewGuid();
            PartId = part;
            StationId = station;
            Date = DateTime.Now;
            Origin = origin;
            Destination = destination;
            Collaborator = collaborator;
        }
    }
}