using Godot;
using System.Collections.Generic;

public abstract class AutomationModel<TCellState> : SimulationModel
{
    #region Properties
    public abstract Vector2I FieldSize { get; }

    public abstract AutomationFieldWrapping FieldWrapping { get; set; }
    #endregion

    #region Methods
    public void SetCellsState(IEnumerable<Vector2I> cells, TCellState state)
    {
        DoSetCellsState(cells, state);
        NotifyStateChanged();
    }

    protected abstract void DoSetCellsState(IEnumerable<Vector2I> cells, TCellState state);
    #endregion
}