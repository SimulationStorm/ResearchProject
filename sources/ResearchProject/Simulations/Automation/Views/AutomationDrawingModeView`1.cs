using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public abstract partial class AutomationDrawingModeView<TCellState> : VBoxContainer, IUnsubscribe
{
    private AutomationDrawingModeVM<TCellState> _viewModel = null!;

    protected void Setup(AutomationDrawingModeVM<TCellState> viewModel)
    {
        _viewModel = viewModel;
        SetupControls();
    }

    #region Controls
    private void SetupControls()
    {
        SetupDrawingModeCheckButton();
        SetupDrawingModeControlsBox();
        SetupBrushRadiusBox();
        SetupBrushShapesBox();
        SetupBrushCellStatesBox();
    }

    #region Drawing mode checkbutton
    [Export] public NodePath DrawingModeCheckButtonPath { get; set; } = null!;
    private RichCheckButton _drawingModeCheckButton = null!;

    private void SetupDrawingModeCheckButton()
    {
        _drawingModeCheckButton = GetNode<RichCheckButton>(DrawingModeCheckButtonPath);
        PropertyBinder.BindTwoWay(this, _drawingModeCheckButton, t => t.IsChecked, _viewModel, s => s.DrawingModeEnabled);
    }
    #endregion

    #region Drawing mode controls box
    [Export] public NodePath DrawingModeControlsBoxPath { get; set; } = null!;
    private Container _drawingModeControlsBox = null!;

    private void SetupDrawingModeControlsBox()
    {
        _drawingModeControlsBox = GetNode<Container>(DrawingModeControlsBoxPath);
        PropertyBinder.BindOneWay(this, _drawingModeControlsBox, t => t.Visible, _viewModel, s => s.DrawingModeEnabled);
    }
    #endregion

    #region Brush radius box
    private void SetupBrushRadiusBox()
    {
        SetupBrushRadiusLabel();
        SetupBrushRadiusSlider();
    }

    #region Brush radius label
    [Export] public NodePath BrushRadiusLabelPath { get; set; } = null!;
    private Label _brushRadiusLabel = null!;

    private void SetupBrushRadiusLabel()
    {
        _brushRadiusLabel = GetNode<Label>(BrushRadiusLabelPath);

        PropertyBinder.BindOneWay(this, _brushRadiusLabel, t => t.Text, _viewModel, s => s.DrawingBrushRadius,
            brushRadius => $"{brushRadius} пикс.");
    }
    #endregion

    #region Brush radius slider
    [Export] public NodePath BrushRadiusSliderPath { get; set; } = null!;
    private RichHSlider _brushRadiusSlider = null!;

    private void SetupBrushRadiusSlider()
    {
        _brushRadiusSlider = GetNode<RichHSlider>(BrushRadiusSliderPath);
        _brushRadiusSlider.MinValue = AutomationSettings.MinDrawingBrushRadius;
        _brushRadiusSlider.MaxValue = AutomationSettings.MaxDrawingBrushRadius;
        _brushRadiusSlider.IntValue = _viewModel.DrawingBrushRadius;

        PropertyBinder.BindOneWay(this, _brushRadiusSlider, t => t.Editable, _viewModel, s => s.DrawingModeEnabled);
        PropertyBinder.BindOneWayToSource(this, _brushRadiusSlider, t => t.IntValue, _viewModel, s => s.DrawingBrushRadius);
    }
    #endregion
    #endregion

    #region Brush shapes box
    [Export] public NodePath BrushShapesBoxPath { get; set; } = null!;
    private Container _brushShapesBox = null!;

    private void SetupBrushShapesBox()
    {
        _brushShapesBox = GetNode<Container>(BrushShapesBoxPath);

        foreach (var brushShape in DrawingBrushShapeExtensions.All)
        {
            var brushShapeButton = new RichButton
            {
                Text = brushShape.Name(),
                FocusMode = FocusModeEnum.None,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };

            CommandBinder.Bind(this, brushShapeButton, _viewModel.SetDrawingBrushShapeCommand, () =>
                DrawingBrushShapeExtensions.ByName(brushShapeButton.Text));

            _brushShapesBox.AddChild(brushShapeButton);
        }
    }
    #endregion

    #region Brush cell states box
    [Export] public NodePath BrushCellStatesBoxPath { get; set; } = null!;
    protected Container BrushCellStatesBox = null!;

    private void SetupBrushCellStatesBox()
    {
        BrushCellStatesBox = GetNode<Container>(BrushCellStatesBoxPath);

        SetupBrushCellStateButtons();
    }

    protected abstract void SetupBrushCellStateButtons();
    #endregion
    #endregion

    public virtual void Unsubscribe()
    {
        PropertyBinder.Unbind(this);
        CommandBinder.Unbind(this);
    }
}