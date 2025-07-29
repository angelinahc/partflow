using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models.dtos
{
    public class CreateStationDto
{
    public required string StationName { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int Order { get; set; }
}
}