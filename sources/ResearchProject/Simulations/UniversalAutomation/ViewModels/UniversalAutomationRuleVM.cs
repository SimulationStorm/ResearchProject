using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public partial class UniversalAutomationRuleVM : ObservableObject, INotifyStateChanged, IUnsubscribe
{
    #region Properties
    #region Main
    public object? State { get; }

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private double _probability = 1;

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private byte _oldState;

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private byte _newState;

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private UniversalAutomationRuleType _ruleType = UniversalAutomationRuleType.Unconditional;
	public UniversalAutomationNeighborhoodVM NeighborhoodVm { get; }

	//public UniversalAutomationRuleNeighborCountsVM NeighborhoodCountsVm { get; }

	//public UniversalAutomationRuleNeighborPositionsVM NeighborPositionsVm { get; }

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private byte _neighborState;

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private ISet<int> _neighborCounts = new HashSet<int>();

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private ISet<Vector2I> _neighborPositions = new HashSet<Vector2I>();
    #endregion

    #region Additional
    public UniversalAutomationRuleType PreviousType { get; private set; }

    private bool _isProbabilitySet;
    public bool IsProbabilitySet
    {
        get => _isProbabilitySet;
        set
        {
            _isProbabilitySet = value;

            if (!_isProbabilitySet)
                Probability = 1;

			OnPropertyChanged();
        }
    }

    public IReadOnlyList<byte> States => _automationModel.States.ToList();

    public IReadOnlyDictionary<byte, string> NamesByState => _presentationModel.NamesByState;
    #endregion
    #endregion

    #region Commands
    public IRelayCommand<UniversalAutomationRuleVM> DeleteCommand { get; }

	public IRelayCommand<UniversalAutomationRuleVM> MoveUpCommand { get; }

	public IRelayCommand<UniversalAutomationRuleVM> MoveDownCommand { get; }
    #endregion

    #region Fields
    private readonly UniversalAutomationModel _automationModel;

    private readonly UniversalAutomationPresentationModel _presentationModel;
    #endregion
    
    public UniversalAutomationRuleVM
	(
        UniversalAutomationRule rule,
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel,
        IRelayCommand<UniversalAutomationRuleVM> deleteCommand,
		IRelayCommand<UniversalAutomationRuleVM> moveUpCommand,
		IRelayCommand<UniversalAutomationRuleVM> moveDownCommand)
    {
        _automationModel = automationModel;
        _presentationModel = presentationModel;

        Probability = rule.Probability;

        RuleType = rule.Type;
        PreviousType = RuleType;
        
        OldState = rule.OldState;
        NewState = rule.NewState;

        if (rule.Type is not UniversalAutomationRuleType.Unconditional)
        {
            NeighborhoodVm = new UniversalAutomationNeighborhoodVM(rule.Neighborhood);
            NeighborState = rule.NeighborState!.Value;

            if (RuleType is UniversalAutomationRuleType.Totalistic)
                NeighborCounts = new HashSet<int>(rule.NeighborCounts!);
            else
                NeighborPositions = new HashSet<Vector2I>(rule.NeighborPositions!);
        }
        else
        {
            NeighborhoodVm = new();
            NeighborState = automationModel.DefaultState;
        }

        //NeighborhoodCountsVm = new(ruleModel.NeighborhoodModel);
        //NeighborPositionsVm = new(ruleModel.NeighborhoodModel);

		DeleteCommand = deleteCommand;
		MoveUpCommand = moveUpCommand;
		MoveDownCommand = moveDownCommand;

        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.States, () => OnPropertyChanged(nameof(States)));
        TriggerBinder.OnPropertyChanged(this, NeighborhoodVm, o => o.State, () => OnPropertyChanged(nameof(State)));
    }

    public UniversalAutomationRule AsRule()
    {
        var ruleBuilder = new UniversalAutomationRuleBuilder()
            .HasOldState(OldState)
            .HasNewState(NewState)
            .HasProbability(Probability);

        if (RuleType is not UniversalAutomationRuleType.Unconditional)
        {
            ruleBuilder
                .HasNeighborhood(NeighborhoodVm.AsNeighborhood())
                .HasNeighborState(NeighborState);

            if (RuleType is UniversalAutomationRuleType.Totalistic)
                ruleBuilder.HasNeighborCounts(NeighborCounts);
            else
                ruleBuilder.HasNeighborPositions(NeighborPositions);
        }

        return ruleBuilder.Build();
    }

    public void Unsubscribe()
    {
        TriggerBinder.Unbind(this);
    }
}