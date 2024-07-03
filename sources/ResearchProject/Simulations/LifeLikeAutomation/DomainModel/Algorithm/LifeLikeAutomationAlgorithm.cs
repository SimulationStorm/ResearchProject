using System.Collections.Generic;
using System.Linq;
using Godot;

public abstract class LifeLikeAutomationAlgorithm
{
    public string Name { get; }

    public LifeLikeAutomationAlgorithm(string name) => Name = name;

    #region Methods
    public abstract void SetCellState(Vector2I cell, LifeLikeAutomationCellState state);

    public abstract void Reset(Vector2I? newFieldSize);

    public abstract void Advance(LifeLikeAutomationRule rule, AutomationFieldWrapping fieldWrapping);
    #endregion

    public static readonly LifeLikeAutomationAlgorithm
        Bitwise = LifeLikeAutomationBitwiseAlgorithm.Instance,
        Smart = LifeLikeAutomationSmartAlgorithm.Instance;

    public static readonly IEnumerable<LifeLikeAutomationAlgorithm> All = new LifeLikeAutomationAlgorithm[]
    {
        Bitwise,
        Smart
    };

    public static LifeLikeAutomationAlgorithm ByName(string name) => All.First(a => a.Name == name);
}
