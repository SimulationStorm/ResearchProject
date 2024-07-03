using System;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public class SimulationManagerVM : ObservableObject, IUnsubscribe
{
    #region Properties
    public SimulationMode SimulationMode => _simulationManagerModel.SimulationMode;

    public SimulationVM? CurrentSimulationVM { get; private set; }
    #endregion

    #region Fields
    private readonly SimulationManagerModel _simulationManagerModel;

    private readonly FieldStateModel _fieldStateModel;

    private readonly PanelStatesModel _panelStatesModel;
    #endregion

    public SimulationManagerVM
    (
        SimulationManagerModel simulationManagerModel,
        FieldStateModel fieldStateModel,
        PanelStatesModel panelStatesModel)
    {
        _fieldStateModel = fieldStateModel;
        _panelStatesModel = panelStatesModel;
        _simulationManagerModel = simulationManagerModel;

        SwitchSimulationViewModel(SimulationMode);

        TriggerBinder.OnPropertyChanged(this, _simulationManagerModel, o => o.SimulationMode, simulationMode =>
        {
            SwitchSimulationViewModel(simulationMode);
            OnPropertyChanged(nameof(SimulationMode));
        });
    }

    private void SwitchSimulationViewModel(SimulationMode simulationMode)
    {
        CurrentSimulationVM?.Unsubscribe();
        
        CurrentSimulationVM = (SimulationVM)Activator.CreateInstance
        (
            SimulationSettings.SimulationViewModelTypesByMode[simulationMode],
            _simulationManagerModel,
            _fieldStateModel,
            _panelStatesModel,
            _simulationManagerModel.CurrentSimulationModel
        )!;
    }

    public void Unsubscribe()
    {
        CurrentSimulationVM?.Unsubscribe();
        TriggerBinder.Unbind(this);
    }
}