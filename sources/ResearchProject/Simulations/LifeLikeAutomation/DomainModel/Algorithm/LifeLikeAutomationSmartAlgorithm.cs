using System.Collections.Generic;
using Godot;

public class LifeLikeAutomationSmartAlgorithm : LifeLikeAutomationAlgorithm
{
    #region Properties
    public static LifeLikeAutomationSmartAlgorithm Instance { get; } = new();
    
    public IReadOnlySet<Vector2I> AliveCells => _field;
    #endregion

    private LifeLikeAutomationSmartAlgorithm()
        : base("Умный (просчёт только живых клеток)") { }

    #region Fields
    private int _columns,
                _rows;

    private HashSet<Vector2I> _field = new();

    private readonly IDictionary<Vector2I, int> _neighborCountsByCell = new Dictionary<Vector2I, int>();

    private AutomationFieldWrapping _fieldWrapping;
    #endregion

    #region Methods
    public override void SetCellState(Vector2I cell, LifeLikeAutomationCellState state)
    {
        if (state is LifeLikeAutomationCellState.Alive)
            _field.Add(cell);
        else
            _field.Remove(cell);
    }

    public override void Reset(Vector2I? newFieldSize)
    {
        if (newFieldSize is not null)
        {
            _columns = newFieldSize.Value.X;
            _rows = newFieldSize.Value.Y;
        }

        _field.Clear();
    }

    public override void Advance(LifeLikeAutomationRule rule, AutomationFieldWrapping fieldWrapping)
    {
        _fieldWrapping = fieldWrapping;

        CountCellsNeighbors();
        ApplyRule(rule);
    }

    private void CountCellsNeighbors()
    {
        _neighborCountsByCell.Clear();

        foreach (var cell in _field)
            CountCellNeighbors(cell);
    }

    private void ApplyRule(LifeLikeAutomationRule rule)
    {
        var nextField = new HashSet<Vector2I>();
        foreach (var (cell, neighborCount) in _neighborCountsByCell)
        {
            var wasAliveBefore = _field.Contains(cell);
            if (rule.IsBornWhen(neighborCount) || rule.IsSurvivalWhen(neighborCount) && wasAliveBefore)
                nextField.Add(cell);
        }

        _field = nextField;
    }

    private void CountCellNeighbors(Vector2I cell)
    {
        int x = cell.X,
            y = cell.Y;

        for (var xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (var yOffset = -1; yOffset <= 1; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0)
                    continue;

                int neighborX = x + xOffset,
                    neighborY = y + yOffset;

                if (!ApplyFieldWrapping(ref neighborX, ref neighborY))
                    continue;
                
                var neighbor = new Vector2I(neighborX, neighborY);
                if (!_neighborCountsByCell.ContainsKey(neighbor))
                    _neighborCountsByCell[neighbor] = 1;
                else
                    _neighborCountsByCell[neighbor]++;
            }
        }
    }

    private bool ApplyFieldWrapping(ref int x, ref int y)
    {
        switch (_fieldWrapping)
        {
            case AutomationFieldWrapping.NoWrap:
                if (x < 0 || x > _columns - 1|| y < 0 || y > _rows - 1)
                    return false;
                break;

            case AutomationFieldWrapping.Horizontal:
                x = (x + _columns) % _columns;
                break;

            case AutomationFieldWrapping.Vertical:
                y = (y + _rows) % _rows;
                break;

            case AutomationFieldWrapping.Both:
                x = (x + _columns) % _columns;
                y = (y + _rows) % _rows;
                break;
        }

        return true;
    }
    #endregion
}