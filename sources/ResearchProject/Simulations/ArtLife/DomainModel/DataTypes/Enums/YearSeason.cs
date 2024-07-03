using Godot;
using System;
using System.Collections.Generic;

public enum YearSeason
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public static class YearSeasonExtensions
{
    public static IEnumerable<YearSeason> AllYearSeasons { get; } = Enum.GetValues<YearSeason>();

    public static int DurationInDays(this YearSeason yearSeason) => ArtLifeSettings.YearSeasonDurationInDays[yearSeason];

    public static double MinTemperature(this YearSeason yearSeason) => ArtLifeSettings.YearSeasonMinTemperatures[yearSeason];

    public static double MaxTemperature(this YearSeason yearSeason) => ArtLifeSettings.YearSeasonMaxTemperatures[yearSeason];

    public static string Name(this YearSeason yearSeason) => ArtLifeSettings.YearSeasonNames[yearSeason];

    public static Color Color(this YearSeason yearSeason) => ArtLifeSettings.YearSeasonColors[yearSeason];

    public static string IconPath(this YearSeason yearSeason) => ArtLifeSettings.YearSeasonIconPaths[yearSeason];

    public static YearSeason Next(this YearSeason yearSeason) => yearSeason switch
    {
        YearSeason.Spring => YearSeason.Summer,
        YearSeason.Summer => YearSeason.Autumn,
        YearSeason.Autumn => YearSeason.Winter,
        YearSeason.Winter => YearSeason.Spring,
        _ => throw new NotImplementedException()
    };
}