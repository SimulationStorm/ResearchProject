using System.Collections.Generic;

public class LifeLikeAutomationKindCategory
{
    public string Name { get; }

    public string Description { get; }

    public IEnumerable<LifeLikeAutomationKind> Kinds { get; }

    #region Setting up
    private LifeLikeAutomationKindCategory(string name, string description, IEnumerable<LifeLikeAutomationKind> kinds)
    {
        Name = name;
        Description = description;
        Kinds = kinds;
    }


    #endregion

    private static readonly LifeLikeAutomationKindCategory
        LifeFamily = new
        (
            "Семья 'Жизни'",
            "Похожи на классическую игру 'Жизнь'.",

            new[]
            {
                LifeLikeAutomationKind.GameOfLife,
                LifeLikeAutomationKind.HighLife,
                LifeLikeAutomationKind.LongLife,
                LifeLikeAutomationKind.LifeWithoutDeath
            }
        ),

        Mazes = new
        (
            "Лабиринты",
            "Обрузуют лабиринты или туннели.",

            new[]
            {
                LifeLikeAutomationKind.Maze,
                LifeLikeAutomationKind.Maze2,
                LifeLikeAutomationKind.Maze3,
                LifeLikeAutomationKind.Maze4
            }
        ),

        Rugs = new
        (
            "Ковры",
            "Образуют симметричные узоры.",

            new[]
            {
                LifeLikeAutomationKind.PersianRug,
                LifeLikeAutomationKind.Rug2,
                LifeLikeAutomationKind.Rug3,
                LifeLikeAutomationKind.Rug4,
                LifeLikeAutomationKind.Rug5
            }
        ),

        Others = new
        (
            "Другие",
            "Требуют отнесения к группам...",

            new[]
            {
                LifeLikeAutomationKind.LiveFreeOrDie,
                LifeLikeAutomationKind.Seeds,

                LifeLikeAutomationKind.Ntrees,
                LifeLikeAutomationKind.Diamoeba,
                LifeLikeAutomationKind.DayAndNight,
                LifeLikeAutomationKind.Assimilation,
                LifeLikeAutomationKind.Corals,
                LifeLikeAutomationKind.Coagulation,
                LifeLikeAutomationKind.Majority,
                LifeLikeAutomationKind.Annealing
            }
        );

    public static readonly IEnumerable<LifeLikeAutomationKindCategory> All = new[]
    {
        LifeFamily,
        Mazes,
        Rugs,
        Others
    };
}