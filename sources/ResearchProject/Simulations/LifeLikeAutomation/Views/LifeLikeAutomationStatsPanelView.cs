using Godot;
using LiveChartsCore.Measure;

public partial class LifeLikeAutomationStatsPanelView : SimulationStatsPanelView, IView<LifeLikeAutomationStatsPanelVM>
{
	private LifeLikeAutomationStatsPanelVM _viewModel = null!;

	public void Setup(LifeLikeAutomationStatsPanelVM viewModel)
	{
		_viewModel = viewModel;
		base.Setup(viewModel);

		SetupCharts();
	}

    #region Charts
    private void SetupCharts()
    {
        SetupLineChart();
        SetupLineChartScrollbar();
        SetupColumnChart();
        SetupPieChart();
    }
    
    #region Line chart
	[Export] public NodePath LineChartPath { get; set; } = null!;
	private CartesianChart _lineChart = null!;

	private void SetupLineChart()
	{
		_lineChart = GetNode<CartesianChart>(LineChartPath);

		_lineChart.Series = _viewModel.LineChartVM.Series;
		_lineChart.XAxes = _viewModel.LineChartVM.MainChartXAxes;
		_lineChart.YAxes = _viewModel.LineChartVM.MainChartYAxes;
		_lineChart.UpdateStartedCommand = _viewModel.LineChartVM.ChartUpdatedCommand;
		_lineChart.ZoomMode = ZoomAndPanMode.X;

		_lineChart.DrawMargin = _viewModel.LineChartVM.Margin;
	}
	#endregion

	#region Line chart scrollbar
	[Export] public NodePath LineChartScrollbarPath { get; set; } = null!;
	private CartesianChart _lineChartScrollbar = null!;

	private void SetupLineChartScrollbar()
	{
		_lineChartScrollbar = GetNode<CartesianChart>(LineChartScrollbarPath);

		_lineChartScrollbar.Series = _viewModel.LineChartVM.Series;
		_lineChartScrollbar.XAxes = _viewModel.LineChartVM.ScrollBarChartXAxes;
		_lineChartScrollbar.YAxes = _viewModel.LineChartVM.ScrollbarChartYAxes;
		_lineChartScrollbar.PointerPressedCommand = _viewModel.LineChartVM.PointerDownCommand;
		_lineChartScrollbar.PointerReleasedCommand = _viewModel.LineChartVM.PointerUpCommand;
		_lineChartScrollbar.PointerMoveCommand = _viewModel.LineChartVM.PointerMoveCommand;
		_lineChartScrollbar.Sections = _viewModel.LineChartVM.Thumbs;
		_lineChartScrollbar.TooltipPosition = TooltipPosition.Hidden;

		_lineChartScrollbar.DrawMargin = _viewModel.LineChartVM.Margin;
	}
	#endregion

	#region Column chart
	[Export] public NodePath ColumnChartPath { get; set; } = null!;
	private CartesianChart _columnChart = null!;

	private void SetupColumnChart()
	{
		_columnChart = GetNode<CartesianChart>(ColumnChartPath);

		_columnChart.Series = _viewModel.ColumnChartVM.Series;
		_columnChart.ZoomMode = ZoomAndPanMode.X;
	}
	#endregion

	#region Pie chart
	[Export] public NodePath PieChartPath { get; set; } = null!;
	private PieChart _pieChart = null!;

	private void SetupPieChart()
	{
		_pieChart = GetNode<PieChart>(PieChartPath);

		_pieChart.Series = _viewModel.PieChartVM.Series;
		_pieChart.LegendPosition = LegendPosition.Left;
	}
	#endregion
    #endregion
    
    // TODO: What about disposing chart views?
}