using System;
using F1_Web.Model;

namespace F1_Web.ModelDTO;

public class ConstructorDTO
{
    public string? constructorId { get; set; }
    public string? name { get; set; }
    public string? nationality { get; set; }
    public string? url { get; set; }

    public ConstructorDTO() {}

    public ConstructorDTO(Constructor c) {
        (constructorId, name, nationality, url) = (c.constructorId, c.name, c.nationality, c.url);
    }
}
