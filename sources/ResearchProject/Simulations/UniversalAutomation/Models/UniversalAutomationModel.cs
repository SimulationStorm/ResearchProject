using System.Collections.Generic;
using EasyBindings;
using Godot;

public class UniversalAutomationModel : AutomationModel<byte>
{
    #region Properties
    public override Vector2I FieldSize => _automation.FieldSize;

    public override AutomationFieldWrapping FieldWrapping
    {
        get => _automation.FieldWrapping;
        set => SetProperty(_automation.FieldWrapping, value, _automation, (a, fw) => a.FieldWrapping = fw);
    }
    
    public UniversalAutomationKind? Kind
    {
        get => _automation.Kind;
        set
        {
            if (SetProperty(_automation.Kind, value, _automation, (a, k) => a.Kind = k))
            {
                OnPropertyChanged(nameof(States));
                OnPropertyChanged(nameof(RuleSets));
                NotifyStateChanged();
            }
        }
    }

    public IReadOnlySet<byte> States => _automation.States;

    public IReadOnlyList<UniversalAutomationRuleSet> RuleSets => _automation.RuleSets;

    public byte DefaultState
    {
        get => _automation.DefaultState;
        set => SetProperty(_automation.DefaultState, value, _automation, (a, ds) => a.DefaultState = ds);
    }

    public int RuleSetsNumberOfRepetitions
    {
        get => _automation.RuleSetsRepetitionsNumber;
        set => SetProperty(_automation.RuleSetsRepetitionsNumber, value, _automation, (a, n) => a.RuleSetsRepetitionsNumber = n);
    }

    public int RandomSeed
    {
        get => _automation.RandomSeed;
        set => SetProperty(_automation.RandomSeed, value, _automation, (a, s) => a.RandomSeed = s);
    }

    public IGetCell<byte> FieldCellGetter { get; }

    public IReadOnlySimulationHistory<UniversalAutomationHistoryRecord> History { get; }
    #endregion

    #region Public methods
    public void AddState(byte state)
    {
        _automation.AddState(state);
        OnPropertyChanged(nameof(States));
        NotifyStateChanged();
    }

    public void RemoveState(byte state)
    {
        _automation.RemoveState(state);
        OnPropertyChanged(nameof(States));
        NotifyStateChanged();
    }

    public void AddRuleSet(UniversalAutomationRuleSet ruleSet)
    {
        _automation.AddRuleSet(ruleSet);
        OnPropertyChanged(nameof(RuleSets));
    }

    public void ReplaceRuleSet(int index, UniversalAutomationRuleSet ruleSet)
    {
        _automation.ReplaceRuleSet(index, ruleSet);
        OnPropertyChanged(nameof(RuleSets));
    }

    public void MoveRuleSet(int oldIndex, int newIndex)
    {
        _automation.MoveRuleSet(oldIndex, newIndex);
        OnPropertyChanged(nameof(RuleSets));
    }

    public void RemoveRuleSet(int index)
    {
        _automation.RemoveRuleSet(index);
        OnPropertyChanged(nameof(RuleSets));
    }
    #endregion

    #region Protected methods
    public override bool CanAdvance() => _automation.CanAdvance();

    protected override void DoAdvance() => _automation.Advance();

    protected override void DoReset(Vector2I? newFieldSize) => _automation.Reset(newFieldSize);

    protected override void DoSetCellsState(IEnumerable<Vector2I> cells, byte state) => _automation.SetCellsState(cells, state);
    #endregion

    private readonly UniversalAutomation _automation;

    public UniversalAutomationModel(Vector2I fieldSize)
    {
        _automation = new UniversalAutomation
        (
            fieldSize,
            UniversalAutomationSettings.EmptyState,
            UniversalAutomationSettings.MaxNeighborhoodRadius,
            UniversalAutomationSettings.InitialKind,
            UniversalAutomationSettings.InitialFieldWrapping,
            UniversalAutomationSettings.InitialRandomSeed
        );

        FieldCellGetter = _automation;
        History = _automation.History;
    }

    public override void Unsubscribe() => TriggerBinder.Unbind(this);
}