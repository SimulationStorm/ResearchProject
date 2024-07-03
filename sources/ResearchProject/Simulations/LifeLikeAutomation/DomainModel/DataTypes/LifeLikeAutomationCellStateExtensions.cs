using System.Collections.Generic;
using System;
using System.Linq;

public static class LifeLikeAutomationCellStateExtensions
{
    public static readonly IEnumerable<LifeLikeAutomationCellState> All = Enum.GetValues(typeof(LifeLikeAutomationCellState)).Cast<LifeLikeAutomationCellState>();

    private static readonly IReadOnlyDictionary<LifeLikeAutomationCellState, string> NamesByCellState = new Dictionary<LifeLikeAutomationCellState, string>
    {
        [LifeLikeAutomationCellState.Dead] = "Мёртвая",
        [LifeLikeAutomationCellState.Alive] = "Живая"
    };

    public static string Name(this LifeLikeAutomationCellState cellState) => NamesByCellState[cellState];

    public static LifeLikeAutomationCellState ByName(string name) => NamesByCellState.First(kvPair => kvPair.Value == name).Key;
}