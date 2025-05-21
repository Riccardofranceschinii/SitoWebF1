using System;

namespace F1_Web.Model;

public class Circuit
{
    public string? circuitId { get; set; } 
    public string? circuitName { get; set; }
    public Location? Location { get; set; }
    public string? url { get; set; }
}
