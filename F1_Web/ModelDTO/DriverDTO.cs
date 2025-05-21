using System;
using F1_Web.Model;

namespace F1_Web.ModelDTO;

public class DriverDTO
{
    public string? driverId { get; set; }
    public string? givenName { get; set; }
    public string? familyName { get; set; }
    public string? dateOfBirth { get; set; }
    public string? nationality { get; set; }
    public string? url { get; set; }

    public DriverDTO() {}

    public DriverDTO(Driver d) {
        (driverId, givenName, familyName, dateOfBirth, nationality, url) = 
        (d.driverId, d.givenName, d.familyName, d.dateOfBirth, d.nationality, d.url);
    }
}

