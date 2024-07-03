using EasyBindings;
using Godot;

public partial class BasicInfoPanelView : PanelView, IView<BasicInfoPanelVM>
{
    private BasicInfoPanelVM _viewModel = null!;

    public void Setup(BasicInfoPanelVM viewModel)
    {
        _viewModel = viewModel;
        base.Setup(viewModel, true);

        SetupControls();
        SetupFpsUpdateTimer();
    }

    #region Controls
    private void SetupControls()
    {
        SetupFpsLabel();
        SetupIterationNumberLabel();
        SetupFieldSizeLabel();
        SetupFieldCellCountLabel();
    }

    #region Fps label
    [Export] public NodePath FpsLabelPath { get; set; } = null!;
    private Label _fpsLabel = null!;

    private void SetupFpsLabel() => _fpsLabel = GetNode<Label>(FpsLabelPath);
    #endregion

    #region Iteration number label
    [Export] public NodePath IterationNumberLabelPath { get; set; } = null!;
    private Label _iterationNumberLabel = null!;

    private void SetupIterationNumberLabel()
    {
        _iterationNumberLabel = GetNode<Label>(IterationNumberLabelPath);

        PropertyBinder.BindOneWay(this, _iterationNumberLabel, t => t.Text, _viewModel, s => s.IterationNumber, Converters.IntToString);
    }
    #endregion

    #region Field size label
    [Export] public NodePath FieldSizeLabelPath { get; set; } = null!;
    private Label _fieldSizeLabel = null!;

    private void SetupFieldSizeLabel()
    {
        _fieldSizeLabel = GetNode<Label>(FieldSizeLabelPath);

        PropertyBinder.BindOneWay(this, _fieldSizeLabel, t => t.Text, _viewModel, s => s.FieldSize, Converters.Vector2IToString);
    }
    #endregion

    #region Cell count label
    [Export] public NodePath FieldCellCountLabelPath { get; set; } = null!;
    private Label _fieldCellCountLabel = null!;

    private void SetupFieldCellCountLabel()
    {
        _fieldCellCountLabel = GetNode<Label>(FieldCellCountLabelPath);

        PropertyBinder.BindOneWay(this, _fieldCellCountLabel, t => t.Text, _viewModel, s => s.FieldCellCount, Converters.IntToString);
    }
    #endregion
    #endregion

    #region Fps update timer
    [Export] public NodePath FpsUpdateTimerPath { get; set; } = null!;
    private Timer _fpsUpdateTimer = null!;

    private void SetupFpsUpdateTimer()
    {
        _fpsUpdateTimer = GetNode<Timer>(FpsUpdateTimerPath);
        // Todo: Is it correct to use timer in view?
        _fpsUpdateTimer.Timeout += () => _fpsLabel.Text = $"{Converters.IntToString(_viewModel.Fps)}";
        _fpsUpdateTimer.Start();
    }
    #endregion

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _fpsUpdateTimer.Stop();
        PropertyBinder.Unbind(this);
    }
}