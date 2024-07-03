using Godot;
using LiveChartsCore.Measure;

public partial class UniversalAutomationStatsPanelView : SimulationStatsPanelView, IView<UniversalAutomationStatsPanelVM>
{
	private UniversalAutomationStatsPanelVM _viewModel = null!;

	public void Setup(UniversalAutomationStatsPanelVM viewModel)
	{
		base.Setup(viewModel);
		_viewModel = viewModel;

		SetupPieChart();
	}

	#region Pie chart
	[Export] public NodePath PieChartPath { get; set; } = null!;
	private PieChart _pieChart = null!;

	private void SetupPieChart()
	{
		_pieChart = GetNode<PieChart>(PieChartPath);

		_pieChart.Series = _viewModel.PieChartVm.Series;
		_pieChart.LegendPosition = LegendPosition.Left;
	}
	#endregion
}