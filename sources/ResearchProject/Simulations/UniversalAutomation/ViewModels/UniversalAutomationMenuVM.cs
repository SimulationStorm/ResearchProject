using CommunityToolkit.Mvvm.Input;
using EasyBindings;
using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public partial class UniversalAutomationMenuVM : AutomationMenuVM<UniversalAutomationState>
{
    #region Properties
    #region View models
    public UniversalAutomationDrawingModeVM DrawingModeVm { get; }

    public UniversalAutomationFieldWrappingVM FieldWrappingVm { get; }

    public ObservableCollection<UniversalAutomationStateVM> StateVms { get; } = new();

    public ObservableCollection<UniversalAutomationRuleSetVM> RuleSetVms { get; } = new();
    #endregion

    public UniversalAutomationKind? Kind
    {
        get => _automationModel.Kind;
        set => _automationModel.Kind = value;
    }

    public byte DefaultState
    {
        get => _automationModel.DefaultState;
        set
        {
            _automationModel.DefaultState = value;
            DeleteStateCommand.NotifyCanExecuteChanged();
        }
    }

    public int RuleSetsRepetitionsNumber
    {
        get => _automationModel.RuleSetsNumberOfRepetitions;
        set => _automationModel.RuleSetsNumberOfRepetitions = value;
    }

    public int RandomSeed
    {
        get => _automationModel.RandomSeed;
        set
        {
            _automationModel.RandomSeed = value;
            ResetRandomSeedCommand.NotifyCanExecuteChanged();
        }
    }

    public IReadOnlyList<byte> States => _automationModel.States.ToList();

    public IReadOnlyDictionary<byte, string> NamesByState => _presentationModel.NamesByState;
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanResetRandomSeed))]
    private void ResetRandomSeed() => RandomSeed = 0;
    private bool CanResetRandomSeed() => RandomSeed != 0;

    #region State commands
    [RelayCommand(CanExecute = nameof(CanAddState))]
    private void AddState()
    {
        var state = GetNewState();
        var name = GetNewStateName();
        var color = GetNewStateColor();

        _presentationModel.AddState(state, name, color);
        _automationModel.AddState(state);

        StateVms.Add(CreateStateVm(state));

        AddStateCommand.NotifyCanExecuteChanged();
        DeleteStateCommand.NotifyCanExecuteChanged();
    }
    private bool CanAddState() => _automationModel.States.Count < UniversalAutomationSettings.MaxStatesNumber;


    [RelayCommand(CanExecute = nameof(CanDeleteState))]
    private void DeleteState(UniversalAutomationStateVM stateVm)
    {
        var state = stateVm.State;

        _presentationModel.RemoveState(state);
        _automationModel.RemoveState(state);

        StateVms.Remove(stateVm);

        AddStateCommand.NotifyCanExecuteChanged();
        DeleteStateCommand.NotifyCanExecuteChanged();
    }
    private bool CanDeleteState(UniversalAutomationStateVM stateVm)
    {
        if (_automationModel.States.Count is 1)
            return false;

        var state = stateVm.State;

        if (_automationModel.DefaultState.Equals(state))
            return false;

        return _automationModel.RuleSets.All(ruleSet => ruleSet.Rules.All(rule =>
            rule.OldState != state &&
            rule.NewState != state &&
            rule.NeighborState != state));
    }
    #endregion

    #region Rule set commands
    [RelayCommand]
    private void AddRuleSet()
    {
        var ruleSet = CreateNewRuleSet();

        _automationModel.AddRuleSet(ruleSet);
        RuleSetVms.Add(CreateRuleSetVm(ruleSet));

        NotifyRuleSetCommandsCanExecuteChanged();
    }


    [RelayCommand(CanExecute = nameof(CanDeleteRuleSet))]
    private void DeleteRuleSet(UniversalAutomationRuleSetVM ruleSetVm)
    {
        var index = RuleSetVms.IndexOf(ruleSetVm);

        _automationModel.RemoveRuleSet(index);
        RuleSetVms.RemoveAt(index);
        ruleSetVm.Unsubscribe();

        NotifyRuleSetCommandsCanExecuteChanged();
    }
    private bool CanDeleteRuleSet() => _automationModel.RuleSets.Count > 1;


    [RelayCommand(CanExecute = nameof(CanMoveUpRuleSet))]
    private void MoveUpRuleSet(UniversalAutomationRuleSetVM ruleSetVm)
    {
        var oldIndex = RuleSetVms.IndexOf(ruleSetVm);
        var newIndex = oldIndex - 1;

        _automationModel.MoveRuleSet(oldIndex, newIndex);
        RuleSetVms.Move(oldIndex, newIndex);

        NotifyRuleSetMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveUpRuleSet(UniversalAutomationRuleSetVM ruleSetVm) => RuleSetVms.IndexOf(ruleSetVm) > 0;


    [RelayCommand(CanExecute = nameof(CanMoveDownRuleSet))]
    private void MoveDownRuleSet(UniversalAutomationRuleSetVM ruleSetVm)
    {
        var oldIndex = RuleSetVms.IndexOf(ruleSetVm);
        var newIndex = oldIndex + 1;

        _automationModel.MoveRuleSet(oldIndex, newIndex);
        RuleSetVms.Move(oldIndex, newIndex);

        NotifyRuleSetMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveDownRuleSet(UniversalAutomationRuleSetVM ruleSetVm) => RuleSetVms.IndexOf(ruleSetVm) < RuleSetVms.Count - 1;


    private void NotifyRuleSetCommandsCanExecuteChanged()
    {
        DeleteRuleSetCommand.NotifyCanExecuteChanged();
        NotifyRuleSetMoveCommandsCanExecuteChanged();
    }

    private void NotifyRuleSetMoveCommandsCanExecuteChanged()
    {
        MoveUpRuleSetCommand.NotifyCanExecuteChanged();
        MoveDownRuleSetCommand.NotifyCanExecuteChanged();
    }
    #endregion
    #endregion

    #region Fields
    private readonly UniversalAutomationModel _automationModel;

    private readonly UniversalAutomationPresentationModel _presentationModel;
    #endregion

    #region Setting up
    public UniversalAutomationMenuVM
    (
        PanelStatesModel panelStatesModel,
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel
    )
    : base(panelStatesModel)
    {
        _automationModel = automationModel;
        _presentationModel = presentationModel;

        DrawingModeVm = new UniversalAutomationDrawingModeVM(_automationModel, _presentationModel);
        FieldWrappingVm = new UniversalAutomationFieldWrappingVM(_automationModel);

        foreach (var state in _automationModel.States)
            StateVms.Add(CreateStateVm(state));

        foreach (var ruleSet in _automationModel.RuleSets)
            RuleSetVms.Add(CreateRuleSetVm(ruleSet));

        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.RuleSetsNumberOfRepetitions, () => OnPropertyChanged(nameof(RuleSetsRepetitionsNumber)));
        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.RandomSeed, () => OnPropertyChanged(nameof(RandomSeed)));
        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.DefaultState, () => OnPropertyChanged(nameof(DefaultState)));
        TriggerBinder.OnPropertyChanged(this, _automationModel, o => o.States, () => OnPropertyChanged(nameof(States)));
        
        TriggerBinder.OnCollectionItemPropertyChanged(this, RuleSetVms, ruleSetVm => ruleSetVm.State, (ruleSetVm, _) =>
        {
            _automationModel.ReplaceRuleSet(RuleSetVms.IndexOf(ruleSetVm), ruleSetVm.AsRuleSet());

            DeleteStateCommand.NotifyCanExecuteChanged();
        });
    }
    #endregion

    #region Private methods
    #region State creation
    private UniversalAutomationStateVM CreateStateVm(byte state) => new
    (
        state,
        _presentationModel,
        DeleteStateCommand
    );

    private byte GetNewState()
    {
        for (var state = UniversalAutomationSettings.MinState; state < UniversalAutomationSettings.MaxState; state++)
        {
            if (!_automationModel.States.Contains(state))
                return state;
        }

        throw new InvalidOperationException("There are no more unoccupied state numbers.");
    }

    private string GetNewStateName()
    {
        var sequenceNumber = StateVms.Count + 1;
        var name = $"Состояние #{sequenceNumber}";

        var nameDuplicateCount = StateVms.Count(state => state.Name.StartsWith(name));
        if (nameDuplicateCount > 0)
            name = $"{name} ({nameDuplicateCount})";

        return name;
    }

    private static Color GetNewStateColor() => ColorExtensions.GenerateRandomColor();
    #endregion

    #region Rule set creation
    private UniversalAutomationRuleSetVM CreateRuleSetVm(UniversalAutomationRuleSet ruleSet) => new
    (
        ruleSet,
        _automationModel,
        _presentationModel,
        DeleteRuleSetCommand,
        MoveUpRuleSetCommand,
        MoveDownRuleSetCommand
    );

    private UniversalAutomationRuleSet CreateNewRuleSet() => new UniversalAutomationRuleSetBuilder()
        //.HasName(GetNewRuleSetName())
        .HasApplicationsNumber(1)
        .HasRule(new UniversalAutomationRuleBuilder()
            .HasOldState(DefaultState)
            .HasNewState(DefaultState)
            .Build())
        .Build();

    //private string GetNewRuleSetName()
    //{
    //    var sequenceNumber = RuleSetVms.Count + 1;
    //    var name = $"Набор правил #{sequenceNumber}";

    //    var isNameUnique = RuleSetVms.All(ruleSetVm => ruleSetVm.Name != name);
    //    if (isNameUnique == false)
    //        name = $"{name} (копия)";

    //    return name;
    //}
    #endregion
    #endregion
}