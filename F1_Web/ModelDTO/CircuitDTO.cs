using System;
using F1_Web.Model;

namespace F1_Web.ModelDTO;

public class CircuitDTO
{
    public string? circuitId { get; set; } 
    public string? circuitName { get; set; }
    public string? url { get; set; }
    public LocationDTO? Location { get; set; }

    public CircuitDTO() {}

    public CircuitDTO(Circuit c) {
        (circuitId, circuitName, url) = (c.circuitId, c.circuitName, c.url);
        Location = c.Location != null ? new LocationDTO(c.Location) : null;
    }
}
