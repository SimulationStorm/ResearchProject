using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class LifeLikeAutomation
{
    #region Properties
    public Vector2I FieldSize => new(_columns, _rows);

    private LifeLikeAutomationAlgorithm _algorithm;
    public LifeLikeAutomationAlgorithm Algorithm
    {
        get => _algorithm;
        set
        {
            LifeLikeAutomationAlgorithm
                oldAlgorithm = _algorithm,
                newAlgorithm = value;

            if (oldAlgorithm == newAlgorithm)
                return;

            if (oldAlgorithm is LifeLikeAutomationBitwiseAlgorithm)
                CopyFieldFromBitwiseToSmartAlgorithm();
            else
                CopyFieldFromSmartToBitwiseAlgorithm();

            _algorithm = newAlgorithm;
        }
    }

    private LifeLikeAutomationRule _rule = null!;
    public LifeLikeAutomationRule Rule
    {
        get => _rule;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _rule = value;
            if (_kind?.Rule.Equals(value) is false)
                _kind = null;
        }
    }

    // Should be not null when setting
    private LifeLikeAutomationKind? _kind;
    public LifeLikeAutomationKind? Kind
    {
        get => _kind;
        set
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _kind = value;
            _rule = _kind.Rule;
        }
    }

    public AutomationFieldWrapping FieldWrapping { get; set; }

    public IReadOnlySimulationHistory<LifeLikeAutomationHistoryRecord> History { get; }
    #endregion

    #region Fields
    private int _columns,
                _rows;

    private readonly ISimulationHistory<LifeLikeAutomationHistoryRecord> _history;
    #endregion

    #region Setup
    public LifeLikeAutomation
    (
        Vector2I fieldSize,
        LifeLikeAutomationAlgorithm algorithm,
        LifeLikeAutomationKind kind,
        AutomationFieldWrapping fieldWrapping)
    {
        _algorithm = algorithm;
        _history = new LifeLikeAutomationHistory(this);
        
        History = _history;
        Kind = kind;
        FieldWrapping = fieldWrapping;

        Reset(fieldSize);

        _history.MakeRecord();
    }

    public LifeLikeAutomation
    (
        Vector2I fieldSize,
        LifeLikeAutomationAlgorithm algorithm,
        LifeLikeAutomationRule rule,
        AutomationFieldWrapping fieldWrapping)
    {
        _algorithm = algorithm;
        _history = new LifeLikeAutomationHistory(this);

        History = _history;
        Rule = rule;
        FieldWrapping = fieldWrapping;

        Reset(fieldSize);
        _history.MakeRecord();
    }
    #endregion

    #region Public methods
    public void PlacePattern(Vector2I position, LifeLikeAutomationPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(pattern, nameof(pattern));

        var scheme = pattern.Scheme.Split('\n').Select(row => row.Trim()).ToArray();

        int schemeRows = Math.Min(scheme.Length, _rows),
            schemeColumns = Math.Min(scheme[0].Length, _columns);

        int x = Math.Clamp(position.X - schemeColumns / 2, 0, _columns - schemeColumns),
            y = Math.Clamp(position.Y - schemeRows / 2, 0, _rows - schemeRows);

        var aliveCells = new List<Vector2I>();

        for (var c = 0; c < schemeColumns; c++)
            for (var r = 0; r < schemeRows; r++)
                if (scheme[r][c] is LifeLikeAutomationPattern.AliveCellChar)
                    aliveCells.Add(new(x + c, y + r));

        SetCellsState(aliveCells, LifeLikeAutomationCellState.Alive);

        _history.OverwriteLastRecord();
    }

    public void Reset(Vector2I? newFieldSize = null)
    {
        if (newFieldSize is not null)
        {
            _columns = newFieldSize.Value.X;
            _rows = newFieldSize.Value.Y;
        }

        Algorithm.Reset(newFieldSize);

        _history.Clear();
    }

    public void PopulateRandomly(double liveDensity)
    {
        var aliveCells = new List<Vector2I>();

        for (var x = 0; x < _columns; x++)
            for (var y = 0; y < _rows; y++)
                if (Random.Shared.NextDouble() < liveDensity)
                    aliveCells.Add(new(x, y));

        SetCellsState(aliveCells, LifeLikeAutomationCellState.Alive);

        _history.OverwriteLastRecord();
    }

    public void SetCellsState(IEnumerable<Vector2I> cells, LifeLikeAutomationCellState state)
    {
        foreach (var cell in cells)
            Algorithm.SetCellState(cell, state);

        _history.OverwriteLastRecord();
    }

    public void SetCellState(Vector2I cell, LifeLikeAutomationCellState state)
    {
        Algorithm.SetCellState(cell, state);

        _history.OverwriteLastRecord();
    }

    public void Advance()
    {
        Algorithm.Advance(Rule, FieldWrapping);

        _history.MakeRecord();
    }
    #endregion

    #region Private methods
    private void CopyFieldFromBitwiseToSmartAlgorithm()
    {
        var bitwiseAlgorithm = LifeLikeAutomationBitwiseAlgorithm.Instance;
        var smartAlgorithm = LifeLikeAutomationSmartAlgorithm.Instance;

        smartAlgorithm.Reset(FieldSize);

        int columns = FieldSize.X,
            rows = FieldSize.Y;

        for (var x = 0; x < columns; x++)
            for (var y = 0; y < rows; y++)
                if (bitwiseAlgorithm.GetCellState(new(x, y)) is LifeLikeAutomationCellState.Alive)
                    smartAlgorithm.SetCellState(new(x, y), LifeLikeAutomationCellState.Alive);
    }

    private void CopyFieldFromSmartToBitwiseAlgorithm()
    {
        var smartAlgorithm = LifeLikeAutomationSmartAlgorithm.Instance;
        var bitwiseAlgorithm = LifeLikeAutomationBitwiseAlgorithm.Instance;

        bitwiseAlgorithm.Reset(FieldSize);

        foreach (var (x, y) in smartAlgorithm.AliveCells)
            bitwiseAlgorithm.SetCellState(new(x, y), LifeLikeAutomationCellState.Alive);
    }
    #endregion
}