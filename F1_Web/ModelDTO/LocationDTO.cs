using System;
using F1_Web.Model;

namespace F1_Web.ModelDTO;

public class LocationDTO
{
    public string? lat { get; set; }
    public string? Long { get; set; }
    public string? locality { get; set; }
    public string? country { get; set; }

    public LocationDTO() {}

    public LocationDTO(Location l) {
        (lat, Long, locality, country) = (l.lat, l.Long, l.locality, l.country);
    }
}
