public class UniversalAutomationStatsPanelVM : SimulationStatsPanelVM
{
    #region Properties
    public UniversalAutomationPieChartVM PieChartVm { get; }
    #endregion

    public UniversalAutomationStatsPanelVM
    (
        PanelStatesModel panelStatesModel,
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel
    )
    : base(panelStatesModel)
    {
        PieChartVm = new UniversalAutomationPieChartVM(automationModel, presentationModel);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        PieChartVm.Unsubscribe();
    }
}