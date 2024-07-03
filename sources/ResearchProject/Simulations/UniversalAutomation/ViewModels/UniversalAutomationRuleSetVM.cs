using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyBindings;
using EasyBindings.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

public partial class UniversalAutomationRuleSetVM : ObservableObject, INotifyStateChanged, IUnsubscribe
{
    #region Properties
    public object? State { get; }

    [NotifyPropertyChangedFor(nameof(State))]
    [ObservableProperty]
    private int _applicationsNumber;

    public ObservableCollection<UniversalAutomationRuleVM> RuleVms { get; } = new();
    #endregion

    #region Commands
    public IRelayCommand<UniversalAutomationRuleSetVM> DeleteCommand { get; }

    public IRelayCommand<UniversalAutomationRuleSetVM> MoveUpCommand { get; }

    public IRelayCommand<UniversalAutomationRuleSetVM> MoveDownCommand { get; }

    #region Rule commands
    [RelayCommand]
    private void AddRule()
    {
        RuleVms.Add(CreateRuleVm(CreateNewRule()));

        NotifyRuleCommandsCanExecuteChanged();
    }


    [RelayCommand(CanExecute = nameof(CanDeleteRule))]
    private void DeleteRule(UniversalAutomationRuleVM ruleVm)
    {
        RuleVms.Remove(ruleVm);
        ruleVm.Unsubscribe();

        NotifyRuleCommandsCanExecuteChanged();
    }
    private bool CanDeleteRule(UniversalAutomationRuleVM ruleVm) => RuleVms.Count > 1;


    [RelayCommand(CanExecute = nameof(CanMoveUpRule))]
    private void MoveUpRule(UniversalAutomationRuleVM ruleVm)
    {
        var oldIndex = RuleVms.IndexOf(ruleVm);
        var newIndex = oldIndex - 1;
        RuleVms.Move(oldIndex, newIndex);

        NotifyRuleMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveUpRule(UniversalAutomationRuleVM ruleVm) => RuleVms.IndexOf(ruleVm) > 0;


    [RelayCommand(CanExecute = nameof(CanMoveDownRule))]
    private void MoveDownRule(UniversalAutomationRuleVM ruleVm)
    {
        var oldIndex = RuleVms.IndexOf(ruleVm);
        var newIndex = oldIndex + 1;
        RuleVms.Move(oldIndex, newIndex);

        NotifyRuleMoveCommandsCanExecuteChanged();
    }
    private bool CanMoveDownRule(UniversalAutomationRuleVM ruleVm) => RuleVms.IndexOf(ruleVm) < RuleVms.Count - 1;


    private void NotifyRuleCommandsCanExecuteChanged()
    {
        DeleteRuleCommand.NotifyCanExecuteChanged();
        NotifyRuleMoveCommandsCanExecuteChanged();
    }

    private void NotifyRuleMoveCommandsCanExecuteChanged()
    {
        MoveUpRuleCommand.NotifyCanExecuteChanged();
        MoveDownRuleCommand.NotifyCanExecuteChanged();
    }
    #endregion
    #endregion

    #region Fields
    private readonly UniversalAutomationModel _automationModel;
    
    private readonly UniversalAutomationPresentationModel _presentationModel;
    #endregion

    public UniversalAutomationRuleSetVM
    (
        UniversalAutomationRuleSet ruleSet,
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel,
        IRelayCommand<UniversalAutomationRuleSetVM> deleteCommand,
        IRelayCommand<UniversalAutomationRuleSetVM> moveUpCommand,
        IRelayCommand<UniversalAutomationRuleSetVM> moveDownCommand)
    {
        _automationModel = automationModel;
        _presentationModel = presentationModel;

        ApplicationsNumber = ruleSet.ApplicationsNumber;

        foreach (var ruleVm in ruleSet.Rules)
            RuleVms.Add(CreateRuleVm(ruleVm));
        
        DeleteCommand = deleteCommand;
        MoveUpCommand = moveUpCommand;
        MoveDownCommand = moveDownCommand;

        TriggerBinder.OnCollectionChangedAndItemPropertyChanged(this, RuleVms, NotifyStateChanged, ruleVm => ruleVm.State, NotifyStateChanged);
    }

    public UniversalAutomationRuleSet AsRuleSet() => new UniversalAutomationRuleSetBuilder()
        .HasApplicationsNumber(ApplicationsNumber)
        .HasRules(RuleVms.Select(ruleVm => ruleVm.AsRule()))
        .Build();

    #region Private methods
    #region Rule creation
    private UniversalAutomationRuleVM CreateRuleVm(UniversalAutomationRule rule) => new
    (
        rule,
        _automationModel,
        _presentationModel,
        DeleteRuleCommand,
        MoveUpRuleCommand,
        MoveDownRuleCommand
    );

    private UniversalAutomationRule CreateNewRule() => new UniversalAutomationRuleBuilder()
        .HasOldState(_automationModel.DefaultState)
        .HasNewState(_automationModel.DefaultState)
        .Build();
    #endregion

    private void NotifyStateChanged() => OnPropertyChanged(nameof(State));
    #endregion

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}