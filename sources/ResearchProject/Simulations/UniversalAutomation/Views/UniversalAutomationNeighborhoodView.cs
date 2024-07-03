using EasyBindings;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class UniversalAutomationNeighborhoodView : CollapsiblePanelContainer, IView<UniversalAutomationNeighborhoodVM>
{
    #region Properties
    private UniversalAutomationNeighborhoodVM _viewModel = null!;

    private bool _skipViewModelStateChangedNotification;
    #endregion

    public void Setup(UniversalAutomationNeighborhoodVM viewModel)
    {
        _viewModel = viewModel;
        SetupControls();
    }

    #region Controls
    private void SetupControls()
    {
        SetupRadiusBox();
        SetupTemplateOptionButton();
        SetupPositionButtonsGrid();
        SetupSelectAllPositionsButton();
        SetupResetPositionsButton();
    }

    #region Radius box
    private void SetupRadiusBox()
    {
        SetupRadiusLabel();
        SetupRadiusSlider();
    }

    #region Radius label
    [Export] public NodePath RadiusLabelPath { get; set; } = null!;
    private Label _radiusLabel = null!;

    private void SetupRadiusLabel()
    {
        _radiusLabel = GetNode<Label>(RadiusLabelPath);

        PropertyBinder.BindOneWay(this, _radiusLabel, t => t.Text, _viewModel, s => s.Radius, radius => $"{radius} кл.");
    }
    #endregion

    #region Radius slider
    [Export] public NodePath RadiusSliderPath { get; set; } = null!;
    private RichHSlider _radiusSlider = null!;

    private void SetupRadiusSlider()
    {
        _radiusSlider = GetNode<RichHSlider>(RadiusSliderPath);
        _radiusSlider.MinValue = UniversalAutomationSettings.MinNeighborhoodRadius;
        _radiusSlider.MaxValue = UniversalAutomationSettings.MaxNeighborhoodRadius;
        _radiusSlider.IntValue = _viewModel.Radius;

        PropertyBinder.BindOneWayToSource(this, _radiusSlider, t => t.IntValue, _viewModel, s => s.Radius);
    }
    #endregion
    #endregion

    #region Template option button
    [Export] public NodePath TemplateOptionButtonPath { get; set; } = null!;
    private RichOptionButton _templateOptionButton = null!;

    private void SetupTemplateOptionButton()
    {
        _templateOptionButton = GetNode<RichOptionButton>(TemplateOptionButtonPath);

        _templateOptionButton.Items = UniversalAutomationNeighborhoodTemplate.All
            .Select(template => new OptionItem(template.Name)).Prepend(new OptionItem("Не выбран"));

        UpdateTemplateOptionButtonSelectedIndex();

        _templateOptionButton.ItemSelected += item =>
            _viewModel.Template = item.Text is "Не выбран" ? null : UniversalAutomationNeighborhoodTemplate.ByName(item.Text);

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.Template, UpdateTemplateOptionButtonSelectedIndex);
    }

    private void UpdateTemplateOptionButtonSelectedIndex() =>
        _templateOptionButton.SelectedIndex = UniversalAutomationNeighborhoodTemplate.All.IndexOf(_viewModel.Template) + 1;
    #endregion

    #region Position buttons grid
    [Export] public NodePath PositionButtonsGridPath { get; set; } = null!;
    private GridContainer _positionButtonsGrid = null!;

    private readonly IDictionary<Vector2I, ToggleButton> _buttonsByPosition = new Dictionary<Vector2I, ToggleButton>();

    private void SetupPositionButtonsGrid()
    {
        _positionButtonsGrid = GetNode<GridContainer>(PositionButtonsGridPath);

        GeneratePositionButtons();
        UpdatePositionButtons();

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.Radius, GeneratePositionButtons);
        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.State, UpdatePositionButtons);
    }

    private void GeneratePositionButtons()
    {
        foreach (var (_, button) in _buttonsByPosition)
            TriggerBinder.UnbindPropertyChanged(this, button, o => o.IsToggled);

        _buttonsByPosition.Clear();
        _positionButtonsGrid.RemoveChildren();

        _positionButtonsGrid.Columns = UniversalAutomationNeighborhood.GetSide(_viewModel.Radius);

        UniversalAutomationNeighborhood.IterateOverAllPositions(_viewModel.Radius, (position, isCenter) =>
        {
            var button = new ToggleButton
            {
                CustomMinimumSize = new(28, 28),
                FocusMode = FocusModeEnum.None
            };

            _positionButtonsGrid.AddChild(button);

            if (isCenter)
            {
                button.IsEnabled = false;
                button.TooltipText = "Клетка, для которой применяется условие.";
                return;
            }

            button.Text = "*";
            button.TooltipText = $"Соседняя клетка на позиции ({position.X};{position.Y}).";

            _buttonsByPosition[position] = button;
            TriggerBinder.OnPropertyChanged(this, button, o => o.IsToggled, OnPositionButtonIsToggledChanged);
        });
    }

    private void OnPositionButtonIsToggledChanged(ToggleButton button, bool isToggled)
    {
        _skipViewModelStateChangedNotification = true;

        var position = _buttonsByPosition.First(kv => kv.Value == button).Key;
        _viewModel.SetPosition(position, isToggled);

        UpdateButtonSelfModulate(button);
    }

    private void UpdatePositionButtons()
    {
        if (_skipViewModelStateChangedNotification)
        {
            _skipViewModelStateChangedNotification = false;
            return;
        }

        foreach (var (position, button) in _buttonsByPosition)
        {
            button.SetPressedNoSignal(_viewModel.IsPositionSelected(position));
            UpdateButtonSelfModulate(button);
        }
    }

    private static void UpdateButtonSelfModulate(BaseButton button) => button.ToggleSelfModulate(Colors.Green, Colors.White);
    #endregion

    #region Select all positions button
    [Export] public NodePath SelectAllPositionsButtonPath { get; set; } = null!;
    private RichButton _selectAllPositionsButton = null!;

    private void SetupSelectAllPositionsButton()
    {
        _selectAllPositionsButton = GetNode<RichButton>(SelectAllPositionsButtonPath);
        CommandBinder.Bind(this, _selectAllPositionsButton, _viewModel.SelectAllPositionsCommand);
    }
    #endregion

    #region Reset positions button
    [Export] public NodePath ResetPositionsButtonPath { get; set; } = null!;
    private RichButton _resetPositionsButton = null!;

    private void SetupResetPositionsButton()
    {
        _resetPositionsButton = GetNode<RichButton>(ResetPositionsButtonPath);
        CommandBinder.Bind(this, _resetPositionsButton, _viewModel.ResetPositionsCommand);
    }
    #endregion
    #endregion

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        PropertyBinder.Unbind(this);
        CommandBinder.Unbind(this);
        TriggerBinder.Unbind(this);
    }
}