using System;
using System.Collections.Generic;
using System.Linq;

public enum SimulationMode
{
    LifeLikeAutomation,
    UniversalAutomation,
    ArtLife
}

public static class SimulationModeExtensions
{
    public static readonly IEnumerable<SimulationMode> Modes = Enum.GetValues(typeof(SimulationMode)).Cast<SimulationMode>();
}