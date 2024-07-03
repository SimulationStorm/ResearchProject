using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings.Interfaces;

public class MainVM : ObservableObject, IUnsubscribe
{
    #region View models
    public TopPanelVM TopPanelVM { get; }

    public BasicInfoPanelVM BasicInfoPanelVM { get; }

    public ControlPanelVM ControlPanelVM { get; }

    public HelpPanelVM HelpPanelVM { get; }

    public SettingsPanelVM SettingsPanelVM { get; }

    public SimulationManagerVM SimulationManagerVM { get; }
    #endregion

    public MainVM()
    {
        var fieldStateModel = new FieldStateModel();
        var panelStatesModel = new PanelStatesModel();
        var simulationManagerModel = new SimulationManagerModel(fieldStateModel);

        TopPanelVM = new(simulationManagerModel, panelStatesModel);
        BasicInfoPanelVM = new(simulationManagerModel, fieldStateModel, panelStatesModel);
        ControlPanelVM = new(simulationManagerModel, fieldStateModel, panelStatesModel);
        HelpPanelVM = new(panelStatesModel);
        SettingsPanelVM = new(panelStatesModel);
        SimulationManagerVM = new(simulationManagerModel, fieldStateModel, panelStatesModel);
    }

    public void Unsubscribe()
    {
        TopPanelVM.Unsubscribe();
        BasicInfoPanelVM.Unsubscribe();
        ControlPanelVM.Unsubscribe();
        HelpPanelVM.Unsubscribe();
        SettingsPanelVM.Unsubscribe();
        SimulationManagerVM.Unsubscribe();
    }
}