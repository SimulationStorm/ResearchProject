using System;
using System.Collections.Generic;
using System.Linq;

public class LifeLikeAutomationPattern
{
    #region Properties
    public string Name { get; }

    public string Scheme { get; }

    public const char DeadCellChar = '-',
                      AliveCellChar = 'X';
    #endregion

    #region Setting up
    private LifeLikeAutomationPattern(string name, string scheme)
    {
        ValidateName(name);
        ValidateScheme(scheme);

        Name = name;
        Scheme = scheme;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Should not be empty.", nameof(name));
    }

    private static void ValidateScheme(string scheme)
    {
        ArgumentException.ThrowIfNullOrEmpty(scheme);

        var rows = scheme.Split('\n').Select(row => row.Trim()).ToArray();

        var firstRowLength = rows[0].Length;
        if (rows.Skip(1).Any(row => row.Length != firstRowLength))
            throw new ArgumentException("Every row of scheme should be the same length.", nameof(scheme));

        //if (scheme.Any(chr => chr is not (DeadCellChar or AliveCellChar)))
        //    throw new ArgumentException("Should include only dead and alive cell characters.", nameof(scheme));
    }
    #endregion

    public static readonly LifeLikeAutomationPattern

        // Still life
        Block = new
            (
                "Блок",
                """
		        XX
		        XX
		        """
            ),

        Beehive = new
            (
                "Улей",
                """
                -XX-
                X--X
                -XX-
                """
            ),

        Loaf = new
            (
                "Буханка",
                """
				-XX-
				X--X
				-X-X
				--X-
				"""
            ),

        Tub = new
            (
                "Кадка",
                """
			    -X-
			    X-X
			    -X-
			    """
            ),

        Boat = new
            (
                "Лодка",
                """
				XX-
				X-X
				-X-
				"""
            ),

        Carrier = new
            (
                "Носитель",
                """
				XX--
				X--X
				--XX
				"""
            ),

        // Oscillators
        Blinker = new
            (
                "Мигалка",
                """
			    XXX
			    """
            ),

        FigureEight = new
            (
                "Восьмёрка",
                """
				XX----
				XX-X--
				----X-
				-X----
				--X-XX
				----XX
				"""
            ),

        Octagon2 = new
            (
                "Восьмиугольник",
                """
				---XX---
				--X--X--
				-X----X-
				X------X
				X------X
				-X----X-
				--X--X--
				---XX---
				"""
            ),

        Pentadecathlon = new
            (
                "Пятиборье",
                """
				--X----X--
				XX-XXXX-XX
				--X----X--
				"""
            ),

        SparkCoil = new
            (
                "Катушка зажигания",
                """
				XX----XX
				X-X--X-X
				--X--X--
				X-X--X-X
				XX----XX
				"""
            ),

        Wheel = new
            (
                "Колесо",
                """
				------XX----
				------XX----
				------------
				----XXXX----
				XX-X----X---
				XX-X--X-X---
				---X---XX-XX
				---X-X--X-XX
				----XXXX----
				------------
				----XX------
				----XX------
				"""
            ),

        // Spaceships
        LightweightSpaceship = new
            (
               "Лёгкий",
               """
			   -X--X
			   X----
			   X---X
			   XXXX-
			   """
            ),

        MediumweightSpaceship = new
            (
                "Средний",
                """
                ---X--
                -X---X
                X-----
                X----X
                XXXXX-
                """
            ),

        HeavyweightSpaceship = new
            (
                "Тяжелый",
                """
                ---XX--
                -X----X
                X------
                X-----X
                XXXXXX-
                """
            ),

        Glider = new
            (
                "Планер",
                """
                -X-
                --X
                XXX
                """
            ),

        Loafer = new
            (
                "Бродяга",
                """
                -XX--X-XX
                X--X--XX-
                -X-X-----
                --X------
                --------X
                ------XXX
                -----X---
                ------X--
                -------XX
                """
            ),

        Moon = new
            (
                "Луна",
                """
                -X
                X-
                X-
                -X
                """
            ),

        // Guns
        GospelsGliderGun = new
            (
                "Ружьё Госпера",
                """
                ------------------------X-----------
                ----------------------X-X-----------
                ------------XX------XX------------XX
                -----------X---X----XX------------XX
                ----------X-----X---XX--------------
                XX--------X---X-XX----X-X-----------
                XX--------X-----X-------X-----------
                -----------X---X--------------------
                ------------XX----------------------
                """
            ),

        // Methuselahs
        Acorn = new
            (
                "Жёлудь",
                """
                -X-----
                ---X---
                XX--XXX
                """
            ),

        BHeptomino = new
            (
                "Б-Гептомино",
                """
                X-XX
                XXX-
                -X--
                """
            ),

        Diehard = new
            (
                "Твердолобый",
                """
                ------X-
                XX------
                -X---XXX
                """
            ),

        GliderByTheDozen = new
            (
                "Дюжинный планер",
                """
                XX--X
                X---X
                X--XX
                """
            ),

        Piheptomino = new
            (
                "Пихептомино",
                """
                XXX
                X-X
                X-X
                """
            ),

        Thunderbird = new
            (
                "Буревестник",
                """
                XXX
                ---
                -X-
                -X-
                -X-
                """
            ),

        // Wicks
        Ants = new
            (
                "Муравьи",
                """
                XX---XX---XX---XX---XX---XX---XX---XX---XX--
                --XX---XX---XX---XX---XX---XX---XX---XX---XX
                --XX---XX---XX---XX---XX---XX---XX---XX---XX
                XX---XX---XX---XX---XX---XX---XX---XX---XX--
                """
            ),

        BlinkerFuse = new
            (
                "Мигающий предохранитель",
                """
                XX--X-XX-----------------
                XXXXX-X-X----------------
                --------X-XXX-XXX-XXX-XXX
                XXXXX-X-X----------------
                XX--X-XX-----------------
                """
            );

    private static readonly IEnumerable<LifeLikeAutomationPattern> All = new[]
    {
        // Still life's
        Block,
        Beehive,
        Loaf,
        Tub,
        Boat,
        Carrier,

        // Oscillators
        Blinker,
        FigureEight,
        Octagon2,
        Pentadecathlon,
        SparkCoil,
        Wheel,

        // Spaceships
        LightweightSpaceship,
        MediumweightSpaceship,
        HeavyweightSpaceship,
        Glider,
        Loafer,
        Moon,

        // Guns
        GospelsGliderGun,

        // Methuselahs
        Acorn,
        BHeptomino,
        Diehard,
        GliderByTheDozen,
        Piheptomino,
        Thunderbird,

        // Wicks
        Ants,
        BlinkerFuse
    };

    public static LifeLikeAutomationPattern ByName(string name) => All.First(p => p.Name == name);
}
