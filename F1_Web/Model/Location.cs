using System;
using System.Text.Json.Serialization;

namespace F1_Web.Model;

public class Location
{
    public string? lat { get; set; }
    [JsonPropertyName("long")]
    public string? Long { get; set; } 
    public string? locality { get; set; } 
    public string? country { get; set; } 
}
