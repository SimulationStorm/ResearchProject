using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class UniversalAutomation : IGetCell<byte>
{
    #region Properties
    public Vector2I FieldSize => new(_subfieldColumns, _subfieldRows);

    private byte _defaultState;
    public byte DefaultState
    {
        get => _defaultState;
        set => SetDefaultState(value);
    }

    private int _ruleSetsRepetitionsNumber;
    public int RuleSetsRepetitionsNumber
    {
        get => _ruleSetsRepetitionsNumber;
        set => SetRuleSetsRepetitionsNumber(value);
    }

    private UniversalAutomationKind? _kind;
    public UniversalAutomationKind? Kind
    {
        get => _kind;
        set => SetKind(value!);
    }

    public AutomationFieldWrapping FieldWrapping { get; set; }

    private int _randomSeed;
    public int RandomSeed
    {
        get => _randomSeed;
        set
        {
            _randomSeed = value;
            _random = new Random(_randomSeed);
        }
    }

    public IReadOnlySimulationHistory<UniversalAutomationHistoryRecord> History => _history;

    public IReadOnlySet<byte> States => (IReadOnlySet<byte>)_states;

    public IReadOnlyList<UniversalAutomationRuleSet> RuleSets => (IReadOnlyList<UniversalAutomationRuleSet>)_ruleSets;
    #endregion

    #region Fields
    private readonly byte _emptyState;

    private readonly int _subfieldOffset;

    private int
        _fieldColumns,
        _fieldRows,

        _subfieldColumns,
        _subfieldRows,

        _subfieldStartX,
        _subfieldStartY,
        _subfieldEndX,
        _subfieldEndY;

    private bool _isSidesReset = true;

    private byte[,] _field = null!,
                    _tempField = null!;

    private ISet<byte> _states = new HashSet<byte>();

    private IList<UniversalAutomationRuleSet> _ruleSets = new List<UniversalAutomationRuleSet>();

    private Queue<UniversalAutomationRuleSet> _ruleSetQueue = null!;

    private Random _random = null!;

    private readonly ISimulationHistory<UniversalAutomationHistoryRecord> _history;
    #endregion

    #region Setting up

    public UniversalAutomation
    (
        Vector2I fieldSize,
        byte emptyState,
        int maxNeighborhoodRadius,
        UniversalAutomationKind kind,
        AutomationFieldWrapping fieldWrapping,
        int randomSeed)
    {
        _emptyState = emptyState;
        _subfieldOffset = maxNeighborhoodRadius;
        _history = new UniversalAutomationHistory(this);

        SetFieldSize(fieldSize);

        Kind = kind;
        FieldWrapping = fieldWrapping;
        RandomSeed = randomSeed;
        
        _history.MakeRecord();
    }

    public UniversalAutomation
    (
        Vector2I fieldSize,
        byte emptyState,
        int maxNeighborhoodRadius,
        ISet<byte> states,
        byte defaultState,
        IEnumerable<UniversalAutomationRuleSet> ruleSets,
        int ruleSetsRepetitionsNumber,
        AutomationFieldWrapping fieldWrapping,
        int randomSeed)
    {
        _emptyState = emptyState;
        _subfieldOffset = maxNeighborhoodRadius;
        _history = new UniversalAutomationHistory(this);

        SetStates(states);
        DefaultState = defaultState;
        
        SetRuleSets(ruleSets);
        RuleSetsRepetitionsNumber = ruleSetsRepetitionsNumber;
        
        FieldWrapping = fieldWrapping;
        RandomSeed = randomSeed;

        SetFieldSize(fieldSize);
        _history.MakeRecord();
    }
    #endregion

    #region Public methods
    #region State methods
    public void AddState(byte state)
    {
        if (_states.Contains(state))
            throw new ArgumentException("This state was already added.", nameof(state));

        _states.Add(state);

        _kind = null;

        _history.MakeRecord();
    }

    public void RemoveState(byte state)
    {
        if (_states.Count is 1)
            throw new InvalidOperationException("It is not possible to remove last state.");

        if (state == _defaultState)
            throw new InvalidOperationException("It is not possible to remove default state.");

        if (!_states.Remove(state))
            throw new ArgumentException("This state was not added to remove it.", nameof(state));

        _states.Remove(state);

        IterateOverSubfield((x, y) =>
        {
            if (_field[x, y] == state)
                _field[x, y] = _defaultState;
        });

        _kind = null;

        _history.MakeRecord();
    }
    #endregion

    #region Rule set methods
    public void AddRuleSet(UniversalAutomationRuleSet ruleSet)
    {
        _ruleSets.Add(ruleSet);

        GenerateRuleSetQueue();
        _kind = null;
    }

    public void ReplaceRuleSet(int index, UniversalAutomationRuleSet ruleSet)
    {
        ValidateRuleSetIndex(index);

        _ruleSets[index] = ruleSet;

        GenerateRuleSetQueue();
        _kind = null;
    }

    public void MoveRuleSet(int oldIndex, int newIndex)
    {
        ValidateRuleSetIndex(oldIndex);
        ValidateRuleSetIndex(newIndex);

        (_ruleSets[newIndex], _ruleSets[oldIndex]) = (_ruleSets[oldIndex], _ruleSets[newIndex]);

        GenerateRuleSetQueue();
        _kind = null;
    }

    public void RemoveRuleSet(int index)
    {
        ValidateRuleSetIndex(index);

        _ruleSets.RemoveAt(index);

        GenerateRuleSetQueue();
        _kind = null;
    }
    #endregion

    public byte GetCell(int x, int y)
    {
        // TODO:
        //(x, y) = TranslateLocalCoordsToGlobalCoords(x, y);
        return _field[(x + _subfieldOffset), (y + _subfieldOffset)];
    }

    public byte GetCellState(int x, int y)
    {
        // TODO:
        //(x, y) = TranslateLocalCoordsToGlobalCoords(x, y);
        return _field[(x + _subfieldOffset), (y + _subfieldOffset)];
    }

    public void SetCellsState(IEnumerable<Vector2I> cells, byte state)
    {
        foreach (var cell in cells)
        {
            var absPos = RelativePositionToAbsolute(cell);
            _field[absPos.X, absPos.Y] = _tempField[absPos.X, absPos.Y] = state;
        }

        _history.OverwriteLastRecord();
    }

    public void SetCellState(Vector2I cell, byte state)
    {
        var absPos = RelativePositionToAbsolute(cell);
        _field[absPos.X, absPos.Y] = _tempField[absPos.X, absPos.Y] = state;

        _history.OverwriteLastRecord();
    }

    public void Reset(Vector2I? newFieldSize)
    {
        if (newFieldSize is not null)
            SetFieldSize(newFieldSize.Value);

        SetSubfieldCellsToDefaultState();
        CopyFieldToTempField();

        _history.Clear();
        _history.MakeRecord();
    }

    public bool CanAdvance() => _ruleSetQueue.Count > 0;

    public void Advance()
    {
        //if (CanAdvance() is false)
        //    GenerateRuleSetQueue();

        ApplyFieldWrapping(AutomationFieldWrapping.Both);

        var ruleSet = _ruleSetQueue.Dequeue();
        foreach (var rule in ruleSet.Rules)
            ApplyRule(rule);

        CopyTempFieldToField();

        _history.MakeRecord();
    }
    #endregion

    #region Private methods
    #region Setters
    private void SetStates(ISet<byte> states)
    {
        ValidateStates(states);
        _states = states;
    }

    private void SetRuleSets(IEnumerable<UniversalAutomationRuleSet> ruleSets)
    {
        ValidateRuleSets(ruleSets);
        _ruleSets = ruleSets.ToList();
    }

    private void SetFieldSize(Vector2I fieldSize)
    {
        ValidateFieldSize(fieldSize.X, fieldSize.Y);

        _subfieldColumns = fieldSize.X;
        _subfieldRows = fieldSize.Y;

        _subfieldStartX = _subfieldOffset;
        _subfieldStartY = _subfieldOffset;

        _subfieldEndX = _subfieldColumns + _subfieldOffset;
        _subfieldEndY = _subfieldRows + _subfieldOffset;

        _fieldColumns = _subfieldColumns + _subfieldOffset * 2;
        _fieldRows = _subfieldRows + _subfieldOffset * 2;

        _field = new byte[_fieldColumns, _fieldRows];

        ResetFieldSides();
        SetSubfieldCellsToDefaultState();
        CopyFieldToTempField();
    }

    private void SetDefaultState(byte state)
    {
        ValidateDefaultState(state);
        _defaultState = state;
    }

    private void SetKind(UniversalAutomationKind kind)
    {
        ArgumentNullException.ThrowIfNull(kind, nameof(kind));

        _kind = kind;

        _states = kind.States.Select(state => state.Number).ToHashSet();
        _defaultState = kind.DefaultState.Number;

        _ruleSets = kind.RuleSets.ToList();
        _ruleSetsRepetitionsNumber = kind.RuleSetsRepetitionsNumber;

        GenerateRuleSetQueue();

        SetSubfieldCellsToDefaultState();
        CopyFieldToTempField();
    }

    private void SetRuleSetsRepetitionsNumber(int repetitionsNumber)
    {
        ValidateRuleSetsRepetitionsNumber(repetitionsNumber);
        _ruleSetsRepetitionsNumber = repetitionsNumber;

        GenerateRuleSetQueue();
    }
    #endregion

    #region Validations
    private static void ValidateFieldSize(int columns, int rows)
    {
        if (columns <= 0)
            throw new ArgumentOutOfRangeException(nameof(columns), "Should be greater than zero.");

        if (rows <= 0)
            throw new ArgumentOutOfRangeException(nameof(columns), "Should be greater than zero.");
    }

    private static void ValidateRuleSetsRepetitionsNumber(int repetitionsNumber)
    {
        if (repetitionsNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(repetitionsNumber), "Should be greater than or equal to zero.");
    }

    private static void ValidateStates(ISet<byte> states)
    {
        ArgumentNullException.ThrowIfNull(states, nameof(states));

        if (states.IsEmpty())
            throw new ArgumentException("Should include at least one state.", nameof(states));
    }

    private void ValidateDefaultState(byte state)
    {
        if (!_states.Contains(state))
            throw new ArgumentException("This state was not added to mark it default.", nameof(state));
    }

    private static void ValidateRuleSets(IEnumerable<UniversalAutomationRuleSet> ruleSets)
    {
        ArgumentNullException.ThrowIfNull(ruleSets, nameof(ruleSets));

        if (ruleSets.IsEmpty())
            throw new ArgumentException("Should include at least one state.", nameof(ruleSets));
    }

    private void ValidateRuleSetIndex(int index)
    {
        if (index < 0 || index > _ruleSets.Count - 1)
            throw new ArgumentOutOfRangeException(nameof(index));
    }
    #endregion

    private void GenerateRuleSetQueue()
    {
        var ruleStream = Enumerable.Repeat(_ruleSets, RuleSetsRepetitionsNumber)
                                   .SelectMany(ruleSets => ruleSets)
                                   .Select(ruleSet => Enumerable.Repeat(ruleSet, ruleSet.ApplicationsNumber))
                                   .SelectMany(ruleSets => ruleSets);

        _ruleSetQueue = new Queue<UniversalAutomationRuleSet>(ruleStream);
    }

    #region Applying rule
    private void ApplyRule(UniversalAutomationRule rule)
    {
        switch (rule.Type)
        {
            case UniversalAutomationRuleType.Unconditional:
                ApplyUnconditionalRule(rule);
                break;

            case UniversalAutomationRuleType.Totalistic:
                ApplyTotalisticRule(rule);
                break;

            case UniversalAutomationRuleType.Nontotalistic:
                ApplyNontotalisticRule(rule);
                break;
        }
    }

    private void ApplyUnconditionalRule(UniversalAutomationRule rule)
    {
        byte oldState = rule.OldState,
             newState = rule.NewState;

        var probability = rule.Probability;

        IterateOverSubfield((x, y) =>
        {
            if (_random.NextDouble() < probability && _field[x, y] == oldState)
                _tempField[x, y] = newState;
        });
    }

    #region Conditional rule
    private void ApplyTotalisticRule(UniversalAutomationRule rule)
    {
        byte oldState = rule.OldState,
             newState = rule.NewState,
             neighborState = rule.NeighborState!.Value;

        var positionShifts = rule.Neighborhood!.SelectedPositions;

        var neighborCounts = rule.NeighborCounts!;

        var probability = rule.Probability;

        IterateOverSubfield((x, y) =>
        {
            if (_random.NextDouble() > probability || _field[x, y] != oldState)
                return;

            var neighborCount = positionShifts.Sum(shift =>
                _field[x + shift.X, y + shift.Y] == neighborState ? 1 : 0);

            if (neighborCounts.Contains(neighborCount))
                _tempField[x, y] = newState;
        });
    }

    private void ApplyNontotalisticRule(UniversalAutomationRule rule)
    {
        byte oldState = rule.OldState,
             newState = rule.NewState,
             neighborState = rule.NeighborState!.Value;

        var positionShifts = rule.Neighborhood!.SelectedPositions.Intersect(rule.NeighborPositions!);

        var probability = rule.Probability;

        IterateOverSubfield((x, y) =>
        {
            if (_random.NextDouble() > probability || _field[x, y] != oldState)
                return;

            var areAllNeighborsAtPositionsInGivenState =
                positionShifts.All(shift => _field[x + shift.X, y + shift.Y] == neighborState);

            if (areAllNeighborsAtPositionsInGivenState)
                _tempField[x, y] = newState;
        });
    }
    #endregion
    #endregion

    #region Field wrapping
    private void ApplyFieldWrapping(AutomationFieldWrapping fieldWrapping)
    {
        switch (fieldWrapping)
        {
            case AutomationFieldWrapping.NoWrap:
                if (!_isSidesReset)
                {
                    ResetFieldSides();
                    _isSidesReset = true;
                }
                break;

            case AutomationFieldWrapping.Horizontal:
                CopyFieldHorizontalSides();
                _isSidesReset = false;
                break;

            case AutomationFieldWrapping.Vertical:
                CopyFieldVerticalSides();
                _isSidesReset = false;
                break;

            case AutomationFieldWrapping.Both:
                CopyFieldHorizontalSides();
                CopyFieldVerticalSides();
                _isSidesReset = false;
                break;
        }
    }

    private void CopyFieldHorizontalSides()
    {
        for (var y = _subfieldStartY; y < _subfieldEndY; y++)
        {
            for (var i = 0; i < _subfieldOffset; i++)
            {
                // Copy right to left
                _field[i, y] = _field[_subfieldEndX - _subfieldOffset + i, y];
                // Copy left to right
                _field[_subfieldEndX + i, y] = _field[i + _subfieldOffset, y];
            }
        }
    }

    private void CopyFieldVerticalSides()
    {
        for (var x = _subfieldStartX; x < _subfieldEndX; x++)
        {
            for (var i = 0; i < _subfieldOffset; i++)
            {
                // Copy bottom to top
                _field[x, i] = _field[x, _subfieldEndY - _subfieldOffset + i];
                // Copy top to bottom
                _field[x, _subfieldEndY + i] = _field[x, i + _subfieldOffset];
            }
        }
    }

    private void ResetFieldSides()
    {
        // Reset horizontal sides
        for (var y = _subfieldStartY; y < _subfieldEndY; y++)
        {
            for (var i = 0; i < _subfieldOffset; i++)
            {
                // Reset left side
                _field[i, y] = _emptyState;
                // Reset right side
                _field[_subfieldEndX + i, y] = _emptyState;
            }
        }

        // Reset vertical sides
        for (var x = _subfieldStartX; x < _subfieldEndX; x++)
        {
            for (var i = 0; i < _subfieldOffset; i++)
            {
                //Reset top
                _field[x, i] = _emptyState;
                // Reset bottom
                _field[x, _subfieldEndY + i] = _emptyState;
            }
        }
    }
    #endregion

    #region Iteration over field
    private void SetSubfieldCellsToDefaultState() => SetSubfieldCellsToState(_defaultState);

    private void SetSubfieldCellsToState(byte state) => IterateOverSubfield((x, y) => _field[x, y] = state);

    private void IterateOverSubfield(Action<int, int> action)
    {
        for (var x = _subfieldStartX; x < _subfieldEndX; x++)
            for (var y = _subfieldStartY; y < _subfieldEndY; y++)
                action(x, y);
    }
    #endregion

    private void CopyFieldToTempField() => _tempField = (byte[,])_field.Clone();

    private void CopyTempFieldToField() => _field = (byte[,])_tempField.Clone();

    private Vector2I RelativePositionToAbsolute(Vector2I position) => new(position.X + _subfieldStartX, position.Y + _subfieldStartY);
    #endregion
}