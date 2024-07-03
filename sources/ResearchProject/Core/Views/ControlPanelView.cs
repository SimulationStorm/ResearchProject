using EasyBindings;
using Godot;
using System.Linq;

public partial class ControlPanelView : PanelView, IView<ControlPanelVM>
{
	private ControlPanelVM _viewModel = null!;

	public void Setup(ControlPanelVM viewModel)
	{
		_viewModel = viewModel;
		base.Setup(viewModel, true);

		SetupControls();
	}

	#region Controls
	private void SetupControls()
	{
		SetupSimulationSpeedBox();
		SetupViewScaleBox();
		SetupSkipFramesBox();
		SetupCellSizeBox();
		SetupGridLinesBox();
	}

	#region Simulation speed box
	private void SetupSimulationSpeedBox()
	{
		SetupSimulationSpeedLabel();
		SetupSimulationSpeedMuLabel();
        SetupSimulationSpeedUnlimitedLabel();
        SetupSimulationSpeedSlider();
	}

	#region Simulation speed label
	[Export] public NodePath SimulationSpeedLabelPath { get; set; } = null!;
	private Label _simulationSpeedLabel = null!;

	private void SetupSimulationSpeedLabel()
	{
		_simulationSpeedLabel = GetNode<Label>(SimulationSpeedLabelPath);

        PropertyBinder.BindOneWay(this, _simulationSpeedLabel, t => t.Text, _viewModel, s => s.IterationsPerSecond, Converters.IntToString);
        PropertyBinder.BindOneWay(this, _simulationSpeedLabel, t => t.Visible, _viewModel, s => s.IterationsPerSecond, ips => ips < SimulationSettings.MaxIterationsPerSecond);
    }
    #endregion

    #region Simulation speed measure unit label
    [Export] public NodePath SimulationSpeedMuLabelPath { get; set; } = null!;
    private Label _simulationSpeedMuLabel = null!;

    private void SetupSimulationSpeedMuLabel()
    {
        _simulationSpeedMuLabel = GetNode<Label>(SimulationSpeedMuLabelPath);

        PropertyBinder.BindOneWay(this, _simulationSpeedMuLabel, t => t.Visible, _viewModel, s => s.IterationsPerSecond, ips => ips < SimulationSettings.MaxIterationsPerSecond);
    }
    #endregion

    #region Simulation speed unlimited label
    [Export] public NodePath SimulationSpeedUnlimitedLabelPath { get; set; } = null!;
    private Label _simulationSpeedUnlimitedLabel = null!;

    private void SetupSimulationSpeedUnlimitedLabel()
    {
        _simulationSpeedUnlimitedLabel = GetNode<Label>(SimulationSpeedUnlimitedLabelPath);

        PropertyBinder.BindOneWay(this, _simulationSpeedUnlimitedLabel, t => t.Visible, _viewModel, s => s.IterationsPerSecond, ips => ips > SimulationSettings.MaxIterationsPerSecond);
    }
    #endregion

    #region Simulation speed slider
    [Export] public NodePath SimulationSpeedSliderPath { get; set; } = null!;
	private RichHSlider _simulationSpeedSlider = null!;

	private void SetupSimulationSpeedSlider()
	{
		_simulationSpeedSlider = GetNode<RichHSlider>(SimulationSpeedSliderPath);
		_simulationSpeedSlider.MinValue = SimulationSettings.MinIterationsPerSecond;
		_simulationSpeedSlider.MaxValue = SimulationSettings.MaxIterationsPerSecond + 1;
		_simulationSpeedSlider.Step = SimulationSettings.IterationsPerSecondStep;
		_simulationSpeedSlider.IntValue = _viewModel.IterationsPerSecond;

		PropertyBinder.BindOneWayToSource(this, _simulationSpeedSlider, t => t.IntValue, _viewModel, s => s.IterationsPerSecond);
	}
	#endregion
	#endregion

	#region View scale box
	private void SetupViewScaleBox()
	{
		SetupViewScaleLabel();
		SetupViewScaleSlider();
	}

	#region View scale label
	[Export] public NodePath ViewScaleLabelPath { get; set; } = null!;
	private Label _viewScaleLabel = null!;

	private void SetupViewScaleLabel()
	{
		_viewScaleLabel = GetNode<Label>(ViewScaleLabelPath);

		PropertyBinder.BindOneWay(this, _viewScaleLabel, t => t.Text, _viewModel, s => s.ViewScale, Converters.PercentToString);
	}
	#endregion

	#region View scale slider
	[Export] public NodePath ViewScaleSliderPath { get; set; } = null!;
	private RichHSlider _viewScaleSlider = null!;

	private void SetupViewScaleSlider()
	{
		_viewScaleSlider = GetNode<RichHSlider>(ViewScaleSliderPath);
		_viewScaleSlider.MinValue = FieldCameraSettings.MinViewScale;
		_viewScaleSlider.MaxValue = FieldCameraSettings.MaxViewScale;
		_viewScaleSlider.Step = (int)FieldCameraSettings.ZoomStep;

		PropertyBinder.BindTwoWay(this, _viewScaleSlider, t => t.Value, _viewModel, s => s.ViewScale);
	}
	#endregion
	#endregion

	#region Skip frames box
	private void SetupSkipFramesBox()
	{
		SetupSkipFramesCheckButton();
		SetupSkipFramesLabelsBox();
		SetupSkipFramesSlider();
	}

	#region Skip frames check button
	[Export] public NodePath SkipFramesCheckButtonPath { get; set; } = null!;
	private RichCheckButton _skipFramesCheckButton = null!;

	private void SetupSkipFramesCheckButton()
	{
		_skipFramesCheckButton = GetNode<RichCheckButton>(SkipFramesCheckButtonPath);
		_skipFramesCheckButton.IsChecked = _viewModel.SkipFrames;

		PropertyBinder.BindOneWayToSource(this, _skipFramesCheckButton, t => t.IsChecked, _viewModel, s => s.SkipFrames);
    }
	#endregion

	#region Skip frames labels box
    [Export] public NodePath SkipFramesLabelsBoxPath { get; set; } = null!;
    private Container _skipFramesLabelsBox = null!;

    private void SetupSkipFramesLabelsBox()
    {
        _skipFramesLabelsBox = GetNode<Container>(SkipFramesLabelsBoxPath);

		SetupSkipFramesLabel();
		SetupSkipEveryLabel();
        SetupSkipAllLabel();

        PropertyBinder.BindOneWay(this, _skipFramesLabelsBox, t => t.Visible, _viewModel, s => s.SkipFrames);
    }

    #region Skip frames label
    [Export] public NodePath SkipFramesLabelPath { get; set; } = null!;
	private Label _skipFramesLabel = null!;

	private void SetupSkipFramesLabel()
	{
		_skipFramesLabel = GetNode<Label>(SkipFramesLabelPath);

        PropertyBinder.BindOneWay(this, _skipFramesLabel, t => t.Visible, _viewModel, s => s.FramesToSkip, fts => fts < SimulationSettings.MaxFramesToSkip);
        PropertyBinder.BindOneWay(this, _skipFramesLabel, t => t.Text, _viewModel, s => s.FramesToSkip, Converters.IntToString);
	}
	#endregion

	#region Skip every label
    [Export] public NodePath SkipEveryLabelPath { get; set; } = null!;
	private Label _skipEveryLabel = null!;

    private void SetupSkipEveryLabel()
    {
        _skipEveryLabel = GetNode<Label>(SkipEveryLabelPath);

		PropertyBinder.BindOneWay(this, _skipEveryLabel, t => t.Visible, _viewModel, s => s.FramesToSkip, fts => fts < SimulationSettings.MaxFramesToSkip);
    }
    #endregion

    #region Skip all label
    [Export] public NodePath SkipAllLabelPath { get; set; } = null!;
    private Label _skipAllLabel = null!;

    private void SetupSkipAllLabel()
    {
        _skipAllLabel = GetNode<Label>(SkipAllLabelPath);

        PropertyBinder.BindOneWay(this, _skipAllLabel, t => t.Visible, _viewModel, s => s.FramesToSkip, fts => fts > SimulationSettings.MaxFramesToSkip);
    }
    #endregion
    #endregion

    #region Skip frames slider
    [Export] public NodePath SkipFramesSliderPath { get; set; } = null!;
	private RichHSlider _skipFramesSlider = null!;

	private void SetupSkipFramesSlider()
	{
		_skipFramesSlider = GetNode<RichHSlider>(SkipFramesSliderPath);
		_skipFramesSlider.MinValue = SimulationSettings.MinFramesToSkip;
		_skipFramesSlider.MaxValue = SimulationSettings.MaxFramesToSkip + 1;
		_skipFramesSlider.Step = SimulationSettings.FramesToSkipStep;
		_skipFramesSlider.IntValue = _viewModel.FramesToSkip;

        PropertyBinder.BindOneWay(this, _skipFramesSlider, t => t.Visible, _viewModel, s => s.SkipFrames);
        PropertyBinder.BindOneWayToSource(this, _skipFramesSlider, t => t.IntValue, _viewModel, s => s.FramesToSkip);
	}
	#endregion
	#endregion

	#region Cell size box
	private void SetupCellSizeBox()
	{
		SetupCellSizeLabel();
		SetupCellSizeItemsSlider();
	}

	#region Cell size label
	[Export] public NodePath CellSizeLabelPath { get; set; } = null!;
	private Label _cellSizeLabel = null!;

	private void SetupCellSizeLabel()
	{
		_cellSizeLabel = GetNode<Label>(CellSizeLabelPath);

		PropertyBinder.BindOneWay(this, _cellSizeLabel, t => t.Text, _viewModel, s => s.CellSize,
            cellSize => Converters.Vector2ToString(new Vector2((float)cellSize, (float)cellSize)));
	}
	#endregion

	#region Cell size items slider
	[Export] public NodePath CellSizeItemsSliderPath { get; set; } = null!;
	private ItemsHSlider _cellSizeItemsSlider = null!;

	private void SetupCellSizeItemsSlider()
	{
		_cellSizeItemsSlider = GetNode<ItemsHSlider>(CellSizeItemsSliderPath);
		_cellSizeItemsSlider.Items = _viewModel.AvailableCellSizes.Cast<object>();
		_cellSizeItemsSlider.SelectedIndex = _viewModel.AvailableCellSizes.IndexOf(_viewModel.CellSize);

		PropertyBinder.BindOneWayToSource(this, _cellSizeItemsSlider, t => t.SelectedItem, _viewModel, s => s.CellSize, item => (double)item!);
	}
	#endregion
	#endregion

	#region Grid lines box 
	private void SetupGridLinesBox()
	{
		SetupGridLinesCheckButton();
		SetupGridLinesColorPicker();
	}

	#region Grid lines check button
	[Export] public NodePath GridLinesCheckButtonPath { get; set; } = null!;
	private RichCheckButton _gridLinesCheckButton = null!;

	private void SetupGridLinesCheckButton()
	{
		_gridLinesCheckButton = GetNode<RichCheckButton>(GridLinesCheckButtonPath);
		_gridLinesCheckButton.IsChecked = _viewModel.GridLinesShown;

		PropertyBinder.BindOneWayToSource(this, _gridLinesCheckButton, t => t.IsChecked, _viewModel, s => s.GridLinesShown);
	}
	#endregion

	#region Grid lines color picker button
	[Export] public NodePath GridLinesColorPickerButtonPath { get; set; } = null!;
	private RichColorPickerButton _gridLinesColorPickerButton = null!;

	private void SetupGridLinesColorPicker()
	{
		_gridLinesColorPickerButton = GetNode<RichColorPickerButton>(GridLinesColorPickerButtonPath);
		_gridLinesColorPickerButton.SelectedColor = _viewModel.GridLinesColor;

		PropertyBinder.BindOneWay(this, _gridLinesColorPickerButton, t => t.Visible, _viewModel, s => s.GridLinesShown);
		PropertyBinder.BindOneWayToSource(this, _gridLinesColorPickerButton, t => t.SelectedColor, _viewModel, s => s.GridLinesColor);
	}
	#endregion
	#endregion
	#endregion

	public override void Unsubscribe()
	{
		base.Unsubscribe();
		PropertyBinder.Unbind(this);
		TriggerBinder.Unbind(this);
		CommandBinder.Unbind(this);
	}
}