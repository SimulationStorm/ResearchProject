using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ArtLifeFieldView : SimulationFieldView
{
    #region Fields
    private ArtLifeFieldVM _viewModel = null!;

    private IGetCell<Cell> _field = null!;
    #endregion

    public void Setup(ArtLifeFieldVM viewModel)
    {
        base.Setup(viewModel);
        _viewModel = viewModel;

        _field = viewModel.Field;
    }

    protected override Color GetCellColor(int x, int y)
    {
        var cell = _field.GetCell(x, y);

        // Задать приоритеты каждому из четырёх способов. И комбинировать их (убрать сдвоенные).
        return _viewModel.DisplayMode switch
        {
            DisplayMode.Substances => GetSubstancesColor(cell),
            DisplayMode.Temperature => GetTemperatureColor(cell),
            DisplayMode.Acidity => GetAcidityColor(cell),
            DisplayMode.Creatures => GetCreatureColor(cell.Creature),
            DisplayMode.SubstancesAndCreatures => GetSubstancesAndCreatureColor(cell),
            DisplayMode.TemperatureAndCreatures => GetTemperatureAndCreatureColor(cell),

            _ => throw new NotImplementedException(),
        };
    }

    #region Temperature color
    private static Color GetTemperatureColor(Cell cell) => cell.Temperature switch
    {
        > 0 => Colors.Red with { A = (float)(cell.Temperature / ArtLifeSettings.MaxPossibleTemperature) },
        < 0 => Colors.Blue with { A = (float)(cell.Temperature / ArtLifeSettings.MinPossibleTemperature) },
        _ => default
    };
    #endregion

    #region Acidity color
    private static readonly IDictionary<int, Color> AcidityColorTable = new Dictionary<int, Color>()
    {
        [0] = Color.Color8(238, 28, 37),
        [1] = Color.Color8(242, 103, 36),
        [2] = Color.Color8(248, 198, 17),
        [3] = Color.Color8(245, 237, 28),
        [4] = Color.Color8(181, 211, 51),
        [5] = Color.Color8(132, 195, 65),
        [6] = Color.Color8(77, 183, 73),
        [7] = Color.Color8(51, 169, 75),
        [8] = Color.Color8(34, 180, 107),
        [9] = Color.Color8(10, 184, 182),
        [10] = Color.Color8(70, 144, 205),
        [11] = Color.Color8(56, 83, 164),
        [12] = Color.Color8(90, 81, 162),
        [13] = Color.Color8(99, 69, 157),
        [14] = Color.Color8(70, 44, 131)
    };

    private static Color GetAcidityColor(Cell cell)
    {
        var acidityFractionalValue = cell.Acidity;

        var acidityIntegerValue = (int)acidityFractionalValue;

        Color firstColor = AcidityColorTable[acidityIntegerValue],
              secondColor = AcidityColorTable[acidityIntegerValue + 1];

        return firstColor.Lerp(secondColor, (float)(acidityFractionalValue - acidityIntegerValue));
    }
    #endregion

    #region Substances color
    private Color GetSubstancesColor(Cell cell)
    {
        var displayedSubstances = _viewModel.DisplayedSubstances;
        var substanceAndAmountsToDisplay = cell.Substances.IntersectBy(displayedSubstances, kv => kv.Key);

        var totalSubstancesAmount = substanceAndAmountsToDisplay.Sum(kv => kv.Value);

        var resultingColor = default(Color);
        foreach (var substanceAndAmount in substanceAndAmountsToDisplay)
            resultingColor = resultingColor.Blend(GetRelativeSubstanceColor(substanceAndAmount.Key, substanceAndAmount.Value, totalSubstancesAmount));

        return resultingColor;
    }

    private static Color GetRelativeSubstanceColor(Substance substance, double amount, double totalSubstancesAmount) =>
        substance.Color() with { A = (float)(amount / totalSubstancesAmount) };
    #endregion

    #region Creature color
    private static Color GetCreatureColor(Creature? creature)
    {
        if (creature == null)
            return default;

        if (creature.Properties.LiveState == CreatureLiveState.Alive)
            return Colors.Green;

        return Colors.Black;
    }
    #endregion

    #region Substances and creature color
    private Color GetSubstancesAndCreatureColor(Cell cell) => cell.Creature != null ? GetCreatureColor(cell.Creature) : GetSubstancesColor(cell);
    #endregion

    #region Temperature and creature color
    private Color GetTemperatureAndCreatureColor(Cell cell) => cell.Creature != null ? GetCreatureColor(cell.Creature) : GetTemperatureColor(cell);
    #endregion
}