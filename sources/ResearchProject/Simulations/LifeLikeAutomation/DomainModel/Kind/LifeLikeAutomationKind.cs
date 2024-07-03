using System;
using System.Collections.Generic;
using System.Linq;

public class LifeLikeAutomationKind
{
    public string Name { get; }

    public LifeLikeAutomationRule Rule { get; }

    #region Setting up
    private LifeLikeAutomationKind(string name, string rule)
    {
        Name = name ?? throw new ArgumentNullException(name);
        Rule = LifeLikeAutomationRule.FromString(rule);
    }


    #endregion

    public static readonly LifeLikeAutomationKind
        // Life family
        GameOfLife = new
        (
            "Игра жизнь",
            "b3/s23"
        ),
        HighLife = new
        (
            "Высокоразвитая жизнь",
            "b36/s23"
        ),
        LongLife = new
        (
            "Продолжительная жизнь",
            "b345/s5"
        ),
        LifeWithoutDeath = new
        (
            "Жизнь без смерти",
            "b3/s012345678"
        ),

        // Mazes
        Maze = new
        (
            "Лабиринт #1",
            "b3/s12345"
        ),
        Maze2 = new
        (
            "Лабиринт #2",
            "b3/s1234"
        ),
        Maze3 = new
        (
            "Лабиринт #3",
            "b37/s12345"
        ),
        Maze4 = new
        (
            "Лабиринт #4",
            "b37/s1234"
        ),

        // Rugs
        PersianRug = new
        (
            "Персидский ковёр",
            "b234/s"
        ),
        Rug2 = new
        (
            "Ковёр #2",
            "b234678/s8"
        ),
        Rug3 = new
        (
            "Ковёр #3",
            "b2345678/s0238"
        ),
        Rug4 = new
        (
            "Ковёр #4",
            "b234567/s124567"
        ),
        Rug5 = new
        (
            "Ковёр #5",
            "b235678/s1234567"
        ),

        // Others
        LiveFreeOrDie = new
        (
            "Живи свободным или умри",
            "b2/s0"
        ),
        Seeds = new
        (
            "Семена",
            "b2/s"
        ),
        Ntrees = new
        (
            "Эйч-деревья",
            "b1/s012345678"
        ),
        Diamoeba = new
        (
            "Диамёба",
            "b35678/s5678"
        ),
        DayAndNight = new
        (
            "День и ночь",
            "b3678/s34678"
        ),
        Assimilation = new
        (
            "Ассимиляция",
            "b345/s4567"
        ),
        Corals = new
        (
            "Кораллы",
            "b3/s45678"
        ),
        Coagulation = new
        (
            "Коагуляция",
            "b378/s235678"
        ),
        Majority = new
        (
            "Голос большинства",
            "b45678/s5678"
        ),
        Annealing = new
        (
            "Отжиг",
            "b378/s235678"
        );

    private static readonly IEnumerable<LifeLikeAutomationKind> All = new[]
    {
        // Life family
        GameOfLife,
        HighLife,
        LongLife,
        LifeWithoutDeath,

        // Mazes
        Maze,
        Maze2,
        Maze3,
        Maze4,

        // Rugs
        PersianRug,
        Rug2,
        Rug3,
        Rug4,
        Rug5,

        // Others
        LiveFreeOrDie,
        Seeds,
        Ntrees,
        Diamoeba,
        DayAndNight,
        Assimilation,
        Corals,
        Coagulation,
        Majority,
        Annealing
    };

    public static LifeLikeAutomationKind ByName(string name) => All.First(k => k.Name == name);
}