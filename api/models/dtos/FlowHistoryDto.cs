using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.dtos
{
    public class FlowHistoryDto
    {
        public required string FromStationName { get; set; }
        public required string ToStationName { get; set; }
        public DateTime MovementDate { get; set; }
        public required string Responsible { get; set; }
    }
}