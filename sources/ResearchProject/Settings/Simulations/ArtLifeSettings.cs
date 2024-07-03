using System.Collections.Generic;
using System.Linq;
using Godot;

public static class ArtLifeSettings
{
    #region Substances
    public static readonly IReadOnlyDictionary<Substance, double> SubstanceWeights = new Dictionary<Substance, double>
    {
        [Substance.Organics] = 1,
        [Substance.Minerals] = 0.8,
        [Substance.Water] = 0.6,
        [Substance.CarbonDioxide] = 0.4,
        [Substance.Oxygen] = 0.2,
    };
    
    public static readonly IReadOnlyDictionary<Substance, Color> SubstanceColors = new Dictionary<Substance, Color>
    {
        [Substance.Organics] = new("50514F"),
        [Substance.Water] = new("247BA0"),
        [Substance.Minerals] = new("FFE066"),
        [Substance.CarbonDioxide] = new("F25F5C"),
        [Substance.Oxygen] = new("70C1B3")
    };

    public static readonly IReadOnlyDictionary<Substance, string> SubstanceNames = new Dictionary<Substance, string>
    {
        [Substance.Organics] = "Органика",
        [Substance.Water] = "Вода",
        [Substance.Minerals] = "Минералы",
        [Substance.CarbonDioxide] = "Углекислый газ",
        [Substance.Oxygen] = "Кислород"
    };

    // Todo: preload
    private const string SubstanceIconsDirectory = "res://Assets/Icons/ArtLife/Substances/";
    public static readonly IReadOnlyDictionary<Substance, string> SubstanceIconPaths = new Dictionary<Substance, string>
    {
        [Substance.Organics] = $"{SubstanceIconsDirectory}Organics.png",
        [Substance.Water] = $"{SubstanceIconsDirectory}Water.png",
        [Substance.Minerals] = $"{SubstanceIconsDirectory}Minerals.png",
        [Substance.CarbonDioxide] = $"{SubstanceIconsDirectory}CarbonDioxide.png",
        [Substance.Oxygen] = $"{SubstanceIconsDirectory}Oxygen.png"
    };

    // TODO: In essence, exchange amount can be calculated based on weight; id est without special given factor
    public const double SubstanceShareFactor = 1; // Substance exchange amount = (multiplier / substance weight) / neighbors count;
    #endregion

    #region Year seasons
    public static readonly IReadOnlyDictionary<YearSeason, int> YearSeasonDurationInDays = new Dictionary<YearSeason, int>
    {
        [YearSeason.Spring] = 5,
        [YearSeason.Summer] = 5,
        [YearSeason.Autumn] = 5,
        [YearSeason.Winter] = 5
    };

    public static readonly IReadOnlyDictionary<YearSeason, double> YearSeasonMinTemperatures = new Dictionary<YearSeason, double>
    {
        [YearSeason.Spring] = -10,
        [YearSeason.Summer] = 5,
        [YearSeason.Autumn] = 0,
        [YearSeason.Winter] = -30
    };

    public static readonly IReadOnlyDictionary<YearSeason, double> YearSeasonMaxTemperatures = new Dictionary<YearSeason, double>
    {
        [YearSeason.Spring] = 5,
        [YearSeason.Summer] = 20,
        [YearSeason.Autumn] = 10,
        [YearSeason.Winter] = -10
    };

    public static readonly IReadOnlyDictionary<YearSeason, string> YearSeasonNames = new Dictionary<YearSeason, string>
    {
        [YearSeason.Spring] = "Весна",
        [YearSeason.Summer] = "Лето",
        [YearSeason.Autumn] = "Осень",
        [YearSeason.Winter] = "Зима"
    };

    public static readonly IReadOnlyDictionary<YearSeason, Color> YearSeasonColors = new Dictionary<YearSeason, Color>
    {
        [YearSeason.Spring] = Colors.Green,
        [YearSeason.Summer] = Colors.Yellow,
        [YearSeason.Autumn] = Colors.Orange,
        [YearSeason.Winter] = Colors.Blue
    };

    // Todo: preload
    private const string YearSeasonIconsDirectory = "res://Assets/Icons/ArtLife/YearSeasons/";
    public static readonly IReadOnlyDictionary<YearSeason, string> YearSeasonIconPaths = new Dictionary<YearSeason, string>
    {
        [YearSeason.Spring] = $"{YearSeasonIconsDirectory}Spring.png",
        [YearSeason.Summer] = $"{YearSeasonIconsDirectory}Summer.png",
        [YearSeason.Autumn] = $"{YearSeasonIconsDirectory}Autumn.png",
        [YearSeason.Winter] = $"{YearSeasonIconsDirectory}Winter.png"
    };

    public static readonly double MinPossibleTemperature = YearSeasonMinTemperatures.Values.Min(),
                                  MaxPossibleTemperature = YearSeasonMaxTemperatures.Values.Max();

    /// <summary>
    /// ...
    /// Should be positive
    /// </summary>
    public const double TemperatureShareAmount = 1;

    public const int DaysToSmoothTemperature = 1;
    #endregion

    #region Times of day
    public static readonly IReadOnlyDictionary<TimeOfDay, int> TimeOfDayDurationInTicks = new Dictionary<TimeOfDay, int>
    {
        [TimeOfDay.Morning] = 30,
        [TimeOfDay.Day] = 30,
        [TimeOfDay.Evening] = 30,
        [TimeOfDay.Night] = 30
    };

    public static readonly IReadOnlyDictionary<TimeOfDay, string> TimeOfDayNames = new Dictionary<TimeOfDay, string>
    {
        [TimeOfDay.Morning] = "Утро",
        [TimeOfDay.Day] = "День",
        [TimeOfDay.Evening] = "Вечер",
        [TimeOfDay.Night] = "Ночь"
    };

    public static readonly IReadOnlyDictionary<TimeOfDay, Color> TimeOfDayColors = new Dictionary<TimeOfDay, Color>
    {
        [TimeOfDay.Morning] = Colors.LightBlue,
        [TimeOfDay.Day] = Colors.DeepSkyBlue,
        [TimeOfDay.Evening] = Colors.Orange,
        [TimeOfDay.Night] = Colors.Blue
    };

    // Todo: preload
    private const string TimeOfDayIconsDirectory = "res://Assets/Icons/ArtLife/TimesOfDay/";
    public static readonly IReadOnlyDictionary<TimeOfDay, string> TimeOfDayIconPaths = new Dictionary<TimeOfDay, string>
    {
        [TimeOfDay.Morning] = $"{TimeOfDayIconsDirectory}Morning.png",
        [TimeOfDay.Day] = $"{TimeOfDayIconsDirectory}Day.png",
        [TimeOfDay.Evening] = $"{TimeOfDayIconsDirectory}Evening.png",
        [TimeOfDay.Night] = $"{TimeOfDayIconsDirectory}Night.png"
    };
    #endregion

    #region Creatures
    public const int CreatureSatisfactionMaxValue = 100;
    #endregion

    public const double AcidityShareAmount = 1;

    public static readonly SubstancesContainer InitialCellSubstances = new()
    {
        [Substance.Organics] = 5,
        [Substance.Minerals] = 10,
        [Substance.Water] = 20,
        [Substance.Oxygen] = 30,
        [Substance.CarbonDioxide] = 35
    };

    public const DisplayMode InitialCellDisplayMode = DisplayMode.Substances;

    // It should be displayed at least one substance
    public static readonly IEnumerable<Substance> InitialDisplayedSubstances = SubstanceExtensions.AllSubstances;
}