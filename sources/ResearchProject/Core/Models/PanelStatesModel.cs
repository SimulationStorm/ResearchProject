using CommunityToolkit.Mvvm.ComponentModel;

public partial class PanelStatesModel : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private bool _helpPanelShown;

    [ObservableProperty]
    private bool _settingsPanelShown;

    [ObservableProperty]
    private bool _basicInfoPanelShown;

    [ObservableProperty]
    private bool _controlPanelShown;

    [ObservableProperty]
    private bool _simulationMenuShown;

    [ObservableProperty]
    private bool _simulationStatsPanelShown;
    #endregion

    public PanelStatesModel()
    {
        HelpPanelShown = PanelsSettings.InitialHelpWindowShown;
        SettingsPanelShown = PanelsSettings.InitialSettingsPanelShown;
        BasicInfoPanelShown = PanelsSettings.InitialBasicInfoPanelShown;
        ControlPanelShown = PanelsSettings.InitialControlPanelShown;
        SimulationMenuShown = PanelsSettings.InitialSimulationMenuShown;
        SimulationStatsPanelShown = PanelsSettings.InitialSimulationStatsPanelShown;
    }
}