using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using System.Collections.Generic;
using EasyBindings;

public partial class LifeLikeAutomationModel : AutomationModel<LifeLikeAutomationCellState>
{
    #region Properties
    public override Vector2I FieldSize => _automation.FieldSize;

    public override AutomationFieldWrapping FieldWrapping
    {
        get => _automation.FieldWrapping;
        set => SetProperty(_automation.FieldWrapping, value, _automation, (a, fw) => a.FieldWrapping = fw);
    }

    public LifeLikeAutomationAlgorithm Algorithm
    {
        get => _automation.Algorithm;
        set => SetProperty(_automation.Algorithm, value, _automation, (a, alg) => a.Algorithm = alg);
    }

    public LifeLikeAutomationRuleVM RuleVm { get; }

    public LifeLikeAutomationKind? Kind
    {
        get => _automation.Kind;
        set
        {
            if (SetProperty(_automation.Kind, value, _automation, (a, k) => a.Kind = k))
                RuleVm.SetRule(_automation.Kind!.Rule);
        }
    }

    [ObservableProperty]
    private LifeLikeAutomationPattern? _pattern;

    public IReadOnlySimulationHistory<LifeLikeAutomationHistoryRecord> History { get; }
    #endregion

    #region Public methods 
    public void PlacePattern(Vector2I position)
    {
        _automation.PlacePattern(position, Pattern!);
        NotifyStateChanged();
    }

    public void PopulateRandomly(double liveDensity)
    {
        _automation.PopulateRandomly(liveDensity);
        NotifyStateChanged();
    }
    #endregion

    #region Protected methods
    protected override void DoAdvance() => _automation.Advance();

    protected override void DoReset(Vector2I? newFieldSize) => _automation.Reset(newFieldSize);

    protected override void DoSetCellsState(IEnumerable<Vector2I> cells, LifeLikeAutomationCellState state) => _automation.SetCellsState(cells, state);
    #endregion

    private readonly LifeLikeAutomation _automation;

    public LifeLikeAutomationModel(Vector2I fieldSize)
    {
        dynamic kindOrRule;

        if (LifeLikeAutomationSettings.InitialAutomationKind != null)
            kindOrRule = LifeLikeAutomationSettings.InitialAutomationKind;
        else
            kindOrRule = LifeLikeAutomationSettings.InitialAutomationRule!;

        _automation = new(fieldSize, LifeLikeAutomationSettings.InitialAlgorithm, kindOrRule, LifeLikeAutomationSettings.InitialFieldWrapping);

        RuleVm = new(_automation.Rule);
        TriggerBinder.OnPropertyChanged(this, RuleVm, o => o.State, () =>
        {
            _automation.Rule = RuleVm.AsRule();
            OnPropertyChanged(nameof(Kind));
        });

        History = _automation.History;
    }

    public override void Unsubscribe() => TriggerBinder.Unbind(this);
}