using System.Collections.Generic;

public class UniversalAutomationKindCategory
{
    public string Name { get; }

    public string Description { get; }

    public IEnumerable<UniversalAutomationKind> Kinds { get; }

    #region Setting up
    public UniversalAutomationKindCategory(string name, string description, IEnumerable<UniversalAutomationKind> kinds)
    {
        Name = name;
        Description = description;
        Kinds = kinds;
    }
    #endregion

    private static readonly UniversalAutomationKindCategory
        LifeFamily = new
        (
            "Семя Жизни",
            "Похожи на классическую игру 'Жизнь'.",

            new[]
            {
                UniversalAutomationKind.GameOfLife
            }
        ),
        ByStephenWolfram = new
        (
            "От Стивена Вольфрама",
            "Открыты Стивеном Вольфрамом.",

            new UniversalAutomationKind[]
            {
            }
        ),
        Others = new
        (
            "Другие",
            "Требуют группировки.",
            new UniversalAutomationKind[]
            {
                UniversalAutomationKind.WireWorld
            }
        );

    public static readonly UniversalAutomationKindCategory[] All =
    {
        LifeFamily,
        ByStephenWolfram,
        Others
    };
}