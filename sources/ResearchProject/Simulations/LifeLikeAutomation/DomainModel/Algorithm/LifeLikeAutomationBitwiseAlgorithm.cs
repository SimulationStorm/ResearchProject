using System.Collections.Generic;
using Godot;

public class LifeLikeAutomationBitwiseAlgorithm : LifeLikeAutomationAlgorithm
{
    public static LifeLikeAutomationBitwiseAlgorithm Instance { get; } = new();

    private LifeLikeAutomationBitwiseAlgorithm()
        : base("Универсальный (просчёт всех клеток)") { }

    #region Fields
    private int _columns,
                _rows;

    private byte[] _field = null!, // Cells current live states
                   _neighbors = null!; // Neighbors of each cell (on respective positions)

    private bool _isSidesReset = true;

    private readonly HashSet<Vector2I> _aliveCells = new();

    private readonly IDictionary<LifeLikeAutomationRule, byte[]> _cellStatesByRuleLookupTable =
        new Dictionary<LifeLikeAutomationRule, byte[]>();
    #endregion

    #region Methods
    public override void SetCellState(Vector2I cell, LifeLikeAutomationCellState state)
    {
        _field[(cell.Y + 1) * _columns + (cell.X + 1)] = (byte)state;

        if (state is LifeLikeAutomationCellState.Alive)
            _aliveCells.Add(cell);
        else
            _aliveCells.Remove(cell);
    }

    public LifeLikeAutomationCellState GetCellState(Vector2I cell) => 
        (LifeLikeAutomationCellState)_field[(cell.Y + 1) * _columns + (cell.X + 1)];

    public override unsafe void Reset(Vector2I? newFieldSize)
    {
        if (newFieldSize is not null)
        {
            _columns = newFieldSize.Value.X + 2;
            _rows = newFieldSize.Value.Y + 2;

            _field = new byte[_columns * _rows];
            _neighbors = new byte[_columns * _rows];
        }
        else
        {
            fixed (byte* fieldPtr = _field)
            {
                for (var i = 0; i < _columns * _rows; i += 8)
                    *(ulong*)(fieldPtr + i) = 0;
            }
        }

        _aliveCells.Clear();
    }

    public override void Advance(LifeLikeAutomationRule rule, AutomationFieldWrapping fieldWrapping)
    {
        ApplyFieldWrapping(fieldWrapping);
        CountCellNeighbors();
        ApplyRule(rule);
    }

    #region Field wrapping
    private void ApplyFieldWrapping(AutomationFieldWrapping fieldWrapping)
    {
        switch (fieldWrapping)
        {
            case AutomationFieldWrapping.NoWrap:
                if (!_isSidesReset)
                {
                    ResetSides();
                    _isSidesReset = true;
                }
                break;

            case AutomationFieldWrapping.Horizontal:
                CopyHorizontalSides();
                _isSidesReset = false;
                break;

            case AutomationFieldWrapping.Vertical:
                CopyVerticalSides();
                _isSidesReset = false;
                break;

            case AutomationFieldWrapping.Both:
                CopyHorizontalSides();
                CopyVerticalSides();
                _isSidesReset = false;
                break;
        }
    }

    private void CopyHorizontalSides()
    {
        for (var y = 0; y < _rows; y++)
        {
            // Copy right to left
            _field[y * _columns] = _field[(y * _columns) + (_columns - 2)];
            // Copy left to right
            _field[(y * _columns) + (_columns - 1)] = _field[(y * _columns) + 1];
        }
    }

    private void CopyVerticalSides()
    {
        for (var x = 0; x < _columns; x++)
        {
            // Copy bottom to top
            _field[x] = _field[(_rows - 2) * _columns + x];
            // Copy top to bottom
            _field[(_rows - 1) * _columns + x] = _field[_columns + x];
        }
    }

    private void ResetSides()
    {
        for (var y = 0; y < _rows; y++)
        {
            // Reset left
            _field[y * _columns] = 0;
            // Reset right
            _field[(y * _columns) + (_columns - 1)] = 0;
        }

        for (var x = 0; x < _columns; x++)
        {
            // Reset top
            _field[x] = 0;
            // Reset bottom
            _field[(_rows - 1) * _columns + x] = 0;
        }
    }
    #endregion

    private unsafe void CountCellNeighbors()
    {
        fixed (byte* fieldPtr = _field, neighborsPtr = _neighbors)
        {
            for (var i = 0; i < _columns * _rows; i += 8)
                *(ulong*)(neighborsPtr + i) = 0;

            for (var i = _columns + 1; i < (_rows - 1) * _columns - 1; i += 8)
            {
                var ptr = (ulong*)(neighborsPtr + i);
                *ptr += *(ulong*)(fieldPtr + i - _columns - 1);
                *ptr += *(ulong*)(fieldPtr + i - _columns);
                *ptr += *(ulong*)(fieldPtr + i - _columns + 1);
                *ptr += *(ulong*)(fieldPtr + i - 1);
                *ptr += *(ulong*)(fieldPtr + i + 1);
                *ptr += *(ulong*)(fieldPtr + i + _columns - 1);
                *ptr += *(ulong*)(fieldPtr + i + _columns);
                *ptr += *(ulong*)(fieldPtr + i + _columns + 1);
            }
        }
    }

    private void ApplyRule(LifeLikeAutomationRule rule)
    {
        var cellStatesLookupTable = GetCellStatesLookupTableByRule(rule);

        for (var i = _columns + 1; i < (_rows - 1) * _columns; i++)
        {
            byte state = _field[i],
                 neighborCount = _neighbors[i],
                 newState = cellStatesLookupTable[(state << 4) | neighborCount];

            _field[i] = newState;
        }
    }

    private byte[] GetCellStatesLookupTableByRule(LifeLikeAutomationRule rule)
    {
        if (_cellStatesByRuleLookupTable.TryGetValue(rule, out var cellStatesLookupTable))
            return cellStatesLookupTable;

        cellStatesLookupTable = CreateCellStatesLookupTable(rule);
        _cellStatesByRuleLookupTable[rule] = cellStatesLookupTable;

        return cellStatesLookupTable;
    }

    private static byte[] CreateCellStatesLookupTable(LifeLikeAutomationRule rule)
    {
        var cellStatesLookupTable = new byte[32];

        for (var i = 0; i < LifeLikeAutomationRule.RuleLength; i++)
            if (rule.IsBornWhen(i))
                cellStatesLookupTable[i] = 1;

        for (var i = 0; i < LifeLikeAutomationRule.RuleLength; i++)
            if (rule.IsSurvivalWhen(i))
                cellStatesLookupTable[16 + i] = 1;

        return cellStatesLookupTable;
    }
    #endregion
}