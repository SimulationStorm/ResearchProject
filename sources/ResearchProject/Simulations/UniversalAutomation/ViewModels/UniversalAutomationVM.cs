public class UniversalAutomationVM : SimulationVM
{
    #region Properties
    public UniversalAutomationFieldVM FieldVm { get; }

    public UniversalAutomationFieldUiVM FieldUiVm { get; }

    public UniversalAutomationMenuVM MenuVm { get; }

    public UniversalAutomationStatsPanelVM StatsPanelVm { get; }
    #endregion

    public UniversalAutomationVM
    (
        SimulationManagerModel simulationManagerModel,
        FieldStateModel fieldStateModel,
        PanelStatesModel panelStatesModel,
        UniversalAutomationModel automationModel)
    {
        var presentationModel = new UniversalAutomationPresentationModel(automationModel.DefaultState);

        FieldVm = new UniversalAutomationFieldVM(simulationManagerModel, fieldStateModel, automationModel, presentationModel);
        FieldUiVm = new UniversalAutomationFieldUiVM(fieldStateModel, automationModel, presentationModel);
        MenuVm = new UniversalAutomationMenuVM(panelStatesModel, automationModel, presentationModel);
        StatsPanelVm = new UniversalAutomationStatsPanelVM(panelStatesModel, automationModel, presentationModel);
    }

    public override void Unsubscribe()
    {
        FieldVm.Unsubscribe();
        FieldUiVm.Unsubscribe();
        MenuVm.Unsubscribe();
        StatsPanelVm.Unsubscribe();
    }
}