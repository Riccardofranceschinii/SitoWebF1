using System;
using System.Text.Json;
using F1_Web.Model;
using F1_Web.ModelDTO;
using Newtonsoft.Json;

namespace F1_Web.Ergast;

public class ErgastResponse
{
    public MRData? MRData { get; set; }
}

public class MRData
{
    public CircuitTable? CircuitTable { get; set; }
    public DriverTable? DriverTable { get; set; }
    public ConstructorTable? ConstructorTable { get; set; }
}

public class CircuitTable
{
    public List<Circuit>? Circuits { get; set; }
}

public class DriverTable
{
    public List<Driver>? Drivers { get; set; }
}

public class ConstructorTable
{
    public List<Constructor>? Constructors { get; set; }
}