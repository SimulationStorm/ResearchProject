public class LifeLikeAutomationStatsPanelVM : SimulationStatsPanelVM
{
    #region View models
    public LifeLikeAutomationLineChartVM LineChartVM { get; }

    public LifeLikeAutomationColumnChartVM ColumnChartVM { get; }

    public LifeLikeAutomationPieChartVM PieChartVM { get; }
    #endregion

    public LifeLikeAutomationStatsPanelVM
    (
        PanelStatesModel panelStatesModel,
        LifeLikeAutomationModel automationModel,
        LifeLikeAutomationPresentationModel presentationModel
    )
    : base(panelStatesModel)
    {
        LineChartVM = new(automationModel.History);
        ColumnChartVM = new(automationModel.History);
        PieChartVM = new(automationModel.History, presentationModel);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        LineChartVM.Unsubscribe();
        ColumnChartVM.Unsubscribe();
        PieChartVM.Unsubscribe();
    }
}