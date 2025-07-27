using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class FlowHistory
    {
        public Guid FlowId { get; set; }
        public Guid PartId { get; set; }
        public Guid FromStationId { get; set; }
        public Guid ToStationId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public required String Collaborator { get; set; }
    }
}