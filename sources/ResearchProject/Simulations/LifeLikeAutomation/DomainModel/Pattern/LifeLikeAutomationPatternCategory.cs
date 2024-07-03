using System;
using System.Collections.Generic;

public class LifeLikeAutomationPatternCategory
{
    public string Name { get; }

    public string Description { get; }

    public IEnumerable<LifeLikeAutomationPattern> Patterns { get; }

    #region Setting up
    private LifeLikeAutomationPatternCategory(string name, string description, IEnumerable<LifeLikeAutomationPattern> patterns)
    {
        ValidateText(name);
        ValidateText(description);
        ValidatePatterns(patterns);

        Name = name;
        Description = description;
        Patterns = patterns;
    }

    private static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentNullException(nameof(text));
    }

    private static void ValidatePatterns(IEnumerable<LifeLikeAutomationPattern> patterns)
    {
        ArgumentNullException.ThrowIfNull(patterns);

        if (patterns.IsEmpty())
            throw new ArgumentException("Should include at least one pattern.", nameof(patterns));
    }
    #endregion
    
    private static readonly LifeLikeAutomationPatternCategory
        StillLife = new
        (
            "Натюрморты",
            "Стабильные неподвижные формы жизни.",
            new[]
            {
                LifeLikeAutomationPattern.Block,
                LifeLikeAutomationPattern.Beehive,
                LifeLikeAutomationPattern.Loaf,
                LifeLikeAutomationPattern.Tub,
                LifeLikeAutomationPattern.Boat,
                LifeLikeAutomationPattern.Carrier
            }
        ),

        Oscillators = new
        (
            "Осцилляторы",
            "Повторяют свою форму после определённого числа поколений (период осциллятора).",
            new[]
            {
                LifeLikeAutomationPattern.Blinker,
                LifeLikeAutomationPattern.FigureEight,
                LifeLikeAutomationPattern.Octagon2,
                LifeLikeAutomationPattern.Pentadecathlon,
                LifeLikeAutomationPattern.SparkCoil,
                LifeLikeAutomationPattern.Wheel
            }
        ),

        Spaceships = new
        (
            "Космические корабли",
            "Перемещаются в одном направлении сохраняя свою форму.",
            new[]
            {
                LifeLikeAutomationPattern.LightweightSpaceship,
                LifeLikeAutomationPattern.MediumweightSpaceship,
                LifeLikeAutomationPattern.HeavyweightSpaceship,
                LifeLikeAutomationPattern.Glider,
                LifeLikeAutomationPattern.Loafer,
                LifeLikeAutomationPattern.Moon
            }
        ),

        Guns = new
        (
            "Ружья",
            "Генерирут космические корабли.",
            new[]
            {
                LifeLikeAutomationPattern.GospelsGliderGun
            }
        ),

        Methuselahs = new
        (
            "Долгожители",
            "Требуют большого количества поколений для стабилизации.",
            new[]
            {
                LifeLikeAutomationPattern.Acorn,
                LifeLikeAutomationPattern.BHeptomino,
                LifeLikeAutomationPattern.Diehard,
                LifeLikeAutomationPattern.GliderByTheDozen,
                LifeLikeAutomationPattern.Piheptomino,
                LifeLikeAutomationPattern.Thunderbird
            }
        ),

        Wicks = new
        (
            "Фитили",
            "Схлопываются в устойчивую форму.",
            new[]
            {
                LifeLikeAutomationPattern.Ants,
                LifeLikeAutomationPattern.BlinkerFuse
            }
        );

    public static readonly IEnumerable<LifeLikeAutomationPatternCategory> All = new[]
    {
        StillLife,
        Oscillators,
        Spaceships,
        Guns,
        Methuselahs,
        Wicks
    };
}