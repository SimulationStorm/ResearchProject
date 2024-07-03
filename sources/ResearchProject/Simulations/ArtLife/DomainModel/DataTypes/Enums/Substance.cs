using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public enum Substance
{
    Organics,
    Minerals,
    Water,
    CarbonDioxide,
    Oxygen
}

public static class SubstanceExtensions
{
    public static IEnumerable<Substance> AllSubstances { get; } = Enum.GetValues<Substance>();

    public static double Weight(this Substance substance) => ArtLifeSettings.SubstanceWeights[substance];

    public static Color Color(this Substance substance) => ArtLifeSettings.SubstanceColors[substance];

    public static string Name(this Substance substance) => ArtLifeSettings.SubstanceNames[substance];

    public static string IconPath(this Substance substance) => ArtLifeSettings.SubstanceIconPaths[substance];
        
    public static Substance ByName(string name) => ArtLifeSettings.SubstanceNames.First(kv => kv.Value == name).Key;
}