public class LifeLikeAutomationVM : SimulationVM
{
    #region View models
    public LifeLikeAutomationFieldVM FieldVM { get; }

    public LifeLikeAutomationFieldUiVM FieldUiVM { get; }

    public LifeLikeAutomationMenuVM MenuVM { get; }

    public LifeLikeAutomationStatsPanelVM StatsPanelVM { get; }
    #endregion

    public LifeLikeAutomationVM
    (
        SimulationManagerModel simulationManagerModel,
        FieldStateModel fieldStateModel,
        PanelStatesModel panelStatesModel,
        LifeLikeAutomationModel automationModel)
    {
        var presentationModel = new LifeLikeAutomationPresentationModel();

        FieldVM = new(simulationManagerModel, fieldStateModel, automationModel, presentationModel);
        FieldUiVM = new(fieldStateModel, automationModel, presentationModel);
        MenuVM = new(panelStatesModel, automationModel, presentationModel);
        StatsPanelVM = new(panelStatesModel, automationModel, presentationModel);
    }

    public override void Unsubscribe()
    {
        FieldVM.Unsubscribe();
        FieldUiVM.Unsubscribe();
        MenuVM.Unsubscribe();
        StatsPanelVM.Unsubscribe();
    }
}