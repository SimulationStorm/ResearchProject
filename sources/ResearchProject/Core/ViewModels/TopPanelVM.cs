using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyBindings;
using EasyBindings.Interfaces;

public partial class TopPanelVM : ObservableObject, IUnsubscribe
{
    #region Properties
    public SimulationRunningState SimulationRunningState
    {
        get => _simulationManagerModel.SimulationRunningState;
        set => _simulationManagerModel.SimulationRunningState = value;
    }

    public SimulationMode SimulationMode
    {
        get => _simulationManagerModel.SimulationMode;
        set => _simulationManagerModel.SimulationMode = value;
    }

    public bool HelpPanelShown
    {
        get => _panelStatesModel.HelpPanelShown;
        set => _panelStatesModel.HelpPanelShown = value;
    }

    public bool SettingsPanelShown
    {
        get => _panelStatesModel.SettingsPanelShown;
        set => _panelStatesModel.SettingsPanelShown = value;
    }

    public bool BasicInfoPanelShown
    {
        get => _panelStatesModel.BasicInfoPanelShown;
        set => _panelStatesModel.BasicInfoPanelShown = value;
    }

    public bool ControlPanelShown
    {
        get => _panelStatesModel.ControlPanelShown;
        set => _panelStatesModel.ControlPanelShown = value;
    }

    public bool SimulationMenuShown
    {
        get => _panelStatesModel.SimulationMenuShown;
        set => _panelStatesModel.SimulationMenuShown = value;
    }

    public bool SimulationStatsPanelShown
    {
        get => _panelStatesModel.SimulationStatsPanelShown;
        set => _panelStatesModel.SimulationStatsPanelShown = value;
    }
    #endregion

    #region Commands
    [RelayCommand]
    private void QuitApplication() => App.Quit();


    [RelayCommand(CanExecute = nameof(CanSetSimulationRunningState))]
    private void SetSimulationRunningState(SimulationRunningState runningState)
    {
        _simulationManagerModel.SimulationRunningState = runningState;
        SetSimulationRunningStateCommand.NotifyCanExecuteChanged();
    }
    private bool CanSetSimulationRunningState(SimulationRunningState runningState) =>
        runningState != _simulationManagerModel.SimulationRunningState;
    

    [RelayCommand]
    private void ResetSimulation() => _simulationManagerModel.ResetSimulation();


    [RelayCommand]
    private void SwitchSimulationMode(SimulationMode newSimulationMode)
    {
        _simulationManagerModel.SwitchSimulationMode(newSimulationMode);
        SwitchSimulationModeCommand.NotifyCanExecuteChanged();
    }
    #endregion

    #region Fields
    private readonly SimulationManagerModel _simulationManagerModel;

    private readonly PanelStatesModel _panelStatesModel;
    #endregion

    public TopPanelVM(SimulationManagerModel simulationManagerModel, PanelStatesModel panelStatesModel)
    {
        _simulationManagerModel = simulationManagerModel;
        TriggerBinder.OnPropertyChanged(this, _simulationManagerModel, o => o.SimulationRunningState, () =>
        {
            OnPropertyChanged(nameof(SimulationRunningState));
            SetSimulationRunningStateCommand.NotifyCanExecuteChanged();
        });

        _panelStatesModel = panelStatesModel;
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.BasicInfoPanelShown, () => OnPropertyChanged(nameof(BasicInfoPanelShown)));
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.ControlPanelShown, () => OnPropertyChanged(nameof(ControlPanelShown)));
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.HelpPanelShown, () => OnPropertyChanged(nameof(HelpPanelShown)));
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.SimulationMenuShown, () => OnPropertyChanged(nameof(SimulationMenuShown)));
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.SimulationStatsPanelShown, () => OnPropertyChanged(nameof(SimulationStatsPanelShown)));
    }

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}