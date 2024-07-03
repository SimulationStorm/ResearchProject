using Godot;
using System;
using System.Collections.Generic;

public enum TimeOfDay
{
    Morning,
    Day,
    Evening,
    Night
}

public static class TimeOfDayExtensions
{
    public static IEnumerable<TimeOfDay> AllTimesOfDay { get; } = Enum.GetValues<TimeOfDay>();

    public static int DurationInTicks(this TimeOfDay timeOfDay) => ArtLifeSettings.TimeOfDayDurationInTicks[timeOfDay];

    public static string Name(this TimeOfDay timeOfDay) => ArtLifeSettings.TimeOfDayNames[timeOfDay];

    public static Color Color(this TimeOfDay timeOfDay) => ArtLifeSettings.TimeOfDayColors[timeOfDay];

    public static string IconPath(this TimeOfDay timeOfDay) => ArtLifeSettings.TimeOfDayIconPaths[timeOfDay];

    public static TimeOfDay Next(this TimeOfDay timeOfDay) => timeOfDay switch
    {
        TimeOfDay.Morning => TimeOfDay.Day,
        TimeOfDay.Day => TimeOfDay.Evening,
        TimeOfDay.Evening => TimeOfDay.Night,
        TimeOfDay.Night => TimeOfDay.Morning,
        _ => throw new NotImplementedException()
    };
}