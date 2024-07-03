using System;
using System.Collections.Generic;
using EasyBindings;
using System.Linq;
using Godot;

public partial class UniversalAutomationRuleView : CollapsiblePanelContainer, IView<UniversalAutomationRuleVM>
{
    private UniversalAutomationRuleVM _viewModel = null!;

    public void Setup(UniversalAutomationRuleVM viewModel)
    {
        _viewModel = viewModel;
        SetupControls();
    }

    #region Controls
    private void SetupControls()
    {
        SetupTopPanelControls();

        //SetupNameBox();

        SetupProbabilityBox();
        SetupSetConditionCheckButton();

        SetupConditionalControls();

        SetupStatesBox();
        SetupNeighborhoodView();
        
        SetupRuleTypeButtons();
        SetupNeighborCountsBox();
        SetupNeighborPositionsGrid();
    }

    #region Top panel controls
    private void SetupTopPanelControls()
    {
        //UpgradeCollapseToggleButton();
        SetupMoveUpButton();
        SetupMoveDownButton();
        SetupDeleteButton();
    }

    #region Collapse toggle button
    //private void UpgradeCollapseToggleButton() => CollapseToggleButton.Text = $" {_viewModel.Name}";
    #endregion

    #region Move up button
    [Export] public NodePath MoveUpButtonPath { get; set; } = null!;
    private RichButton _moveUpButton = null!;

    private void SetupMoveUpButton()
    {
        _moveUpButton = GetNode<RichButton>(MoveUpButtonPath);
        CommandBinder.Bind(this, _moveUpButton, _viewModel.MoveUpCommand, () => _viewModel);
    }
    #endregion

    #region Move down button
    [Export] public NodePath MoveDownButtonPath { get; set; } = null!;
    private RichButton _moveDownButton = null!;

    private void SetupMoveDownButton()
    {
        _moveDownButton = GetNode<RichButton>(MoveDownButtonPath);
        CommandBinder.Bind(this, _moveDownButton, _viewModel.MoveDownCommand, () => _viewModel);
    }
    #endregion

    #region Delete button
    [Export] public NodePath DeleteButtonPath { get; set; } = null!;
    private RichButton _deleteButton = null!;

    private void SetupDeleteButton()
    {
        _deleteButton = GetNode<RichButton>(DeleteButtonPath);
        CommandBinder.Bind(this, _deleteButton, _viewModel.DeleteCommand, () => _viewModel);
    }
    #endregion
    #endregion

    //#region Name box
    //private void SetupNameBox()
    //{
    //    SetupSetNameCheckButton();
    //    SetupNameTextEdit();
    //}

    //#region Set name check button
    //[Export] public NodePath SetNameCheckButtonPath { get; set; } = null!;
    //private RichCheckButton _setNameCheckButton = null!;

    //private void SetupSetNameCheckButton()
    //{
    //    _setNameCheckButton = GetNode<RichCheckButton>(SetNameCheckButtonPath);

    //    //PropertyBinder.BindOneWay(_);
    //}
    //#endregion

    //#region Name text edit
    //[Export] public NodePath NameTextEditPath { get; set; } = null!;
    //private RichTextEdit _nameTextEdit = null!;

    //private void SetupNameTextEdit()
    //{
    //    _nameTextEdit = GetNode<RichTextEdit>(NameTextEditPath);

    //}
    //#endregion
    #endregion

    #region Probability box
    private void SetupProbabilityBox()
    {
        SetupSetProbabilityCheckButton();
        SetupProbabilityLabel();
        SetupProbabilitySlider();
    }

    #region Set probability check button
    [Export] public NodePath SetProbabilityCheckButtonPath { get; set; } = null!;
    private RichCheckButton _setProbabilityCheckButton = null!;

    private void SetupSetProbabilityCheckButton()
    {
        _setProbabilityCheckButton = GetNode<RichCheckButton>(SetProbabilityCheckButtonPath);
        _setProbabilityCheckButton.IsChecked = _viewModel.IsProbabilitySet;

        PropertyBinder.BindOneWayToSource(this, _setProbabilityCheckButton, t => t.IsChecked, _viewModel, s => s.IsProbabilitySet);
    }
    #endregion

    #region Probability label
    [Export] public NodePath ProbabilityLabelPath { get; set; } = null!;
    private Label _probabilityLabel = null!;

    private void SetupProbabilityLabel()
    {
        _probabilityLabel = GetNode<Label>(ProbabilityLabelPath);

        PropertyBinder.BindOneWay(this, _probabilityLabel, t => t.Text, _viewModel, s => s.Probability, probability => $"{(int)(probability * 100)} %");
        PropertyBinder.BindOneWay(this, _probabilityLabel, t => t.SelfModulate, _viewModel, s => s.Probability, Converters.PercentToColor);
        PropertyBinder.BindOneWay(this, _probabilityLabel, t => t.Visible, _viewModel, s => s.IsProbabilitySet);
    }
    #endregion

    #region Probability slider
    [Export] public NodePath ProbabilitySliderPath { get; set; } = null!;
    private RichHSlider _probabilitySlider = null!;

    private void SetupProbabilitySlider()
    {
        _probabilitySlider = GetNode<RichHSlider>(ProbabilitySliderPath);
        _probabilitySlider.MinValue = UniversalAutomationSettings.MinRuleProbability;
        _probabilitySlider.MaxValue = UniversalAutomationSettings.MaxRuleProbability;
        _probabilitySlider.Step = UniversalAutomationSettings.RuleProbabilityStep;

        PropertyBinder.BindTwoWay(this, _probabilitySlider, t => t.Value, _viewModel, s => s.Probability);
        PropertyBinder.BindOneWay(this, _probabilitySlider, t => t.Visible, _viewModel, s => s.IsProbabilitySet);
    }
    #endregion
    #endregion

    #region Set condition check button
    [Export] public NodePath SetConditionCheckButtonPath { get; set; } = null!;
    private RichCheckButton _setConditionCheckButton = null!;

    private void SetupSetConditionCheckButton()
    {
        _setConditionCheckButton = GetNode<RichCheckButton>(SetConditionCheckButtonPath);
        _setConditionCheckButton.IsChecked = _viewModel.RuleType != UniversalAutomationRuleType.Unconditional;

        PropertyBinder.BindOneWayToSource(this, _setConditionCheckButton, t => t.IsChecked, _viewModel, s => s.RuleType,
            isChecked => isChecked
                ? (_viewModel.PreviousType is not UniversalAutomationRuleType.Unconditional
                    ? _viewModel.PreviousType : UniversalAutomationRuleType.Totalistic)
                : UniversalAutomationRuleType.Unconditional);
    }
    #endregion

    #region States box
    private void SetupStatesBox()
    {
        SetupOldStateOptionButton();
        SetupNewStateOptionButton();
        SetupNeighborStateOptionButton();
    }

    #region Old state option button
    [Export] public NodePath OldStateOptionButtonPath { get; set; } = null!;
    private RichOptionButton _oldStateOptionButton = null!;

    private void SetupOldStateOptionButton()
    {
        _oldStateOptionButton = GetNode<RichOptionButton>(OldStateOptionButtonPath);
        SetupStateOptionButton(_oldStateOptionButton, () => _viewModel.OldState, state => _viewModel.OldState = state);
    }
    #endregion

    #region New state option button
    [Export] public NodePath NewStateOptionButtonPath { get; set; } = null!;
    private RichOptionButton _newStateOptionButton = null!;

    private void SetupNewStateOptionButton()
    {
        _newStateOptionButton = GetNode<RichOptionButton>(NewStateOptionButtonPath);
        SetupStateOptionButton(_newStateOptionButton, () => _viewModel.NewState, state => _viewModel.NewState = state);
    }
    #endregion

    #region Neighbor state option button
    [Export] public NodePath NeighborStateOptionButtonPath { get; set; } = null!;
    private RichOptionButton _neighborStateOptionButton = null!;

    private void SetupNeighborStateOptionButton()
    {
        _neighborStateOptionButton = GetNode<RichOptionButton>(NeighborStateOptionButtonPath);
        SetupStateOptionButton(_neighborStateOptionButton, () => _viewModel.NeighborState, state => _viewModel.NeighborState = state);
    }
    #endregion

    private void SetupStateOptionButton
    (
        RichOptionButton optionButton,
        Func<byte> stateGetter,
        Action<byte> stateSetter)
    {
        ResetItemsAndSelectedIndex();

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.States, ResetItemsAndSelectedIndex);

        optionButton.ItemSelected += item =>
        {
            var itemIndex = optionButton.Items.ToList().IndexOf(item);
            var state = _viewModel.States[itemIndex];
            stateSetter(state);
        };

        void ResetItemsAndSelectedIndex()
        {
            var prevSelectedIndex = optionButton.SelectedIndex;

            optionButton.Items = _viewModel.States.Select(state => new OptionItem(_viewModel.NamesByState[state]));

            var maxIndex = _viewModel.States.Count - 1;

            if (prevSelectedIndex is -1)
                optionButton.SelectedIndex = _viewModel.States.IndexOf(stateGetter());
            else if (prevSelectedIndex <= maxIndex)
                optionButton.SelectedIndex = prevSelectedIndex;
            else
                optionButton.SelectedIndex = maxIndex;
        }
    }
    #endregion

    #region Conditional controls
    [Export] public Godot.Collections.Array<Control> ConditionalControls { get; set; } = new();

    private void SetupConditionalControls()
    {
        foreach (var control in ConditionalControls)
            PropertyBinder.BindOneWay(this, control, t => t.Visible, _viewModel, s => s.RuleType,
                ruleType => ruleType is not UniversalAutomationRuleType.Unconditional);
    }
    #endregion

    #region Neighborhood view
    [Export] public NodePath NeighborhoodViewPath { get; set; } = null!;
    private UniversalAutomationNeighborhoodView _neighborhoodView = null!;

    private void SetupNeighborhoodView()
    {
        _neighborhoodView = GetNode<UniversalAutomationNeighborhoodView>(NeighborhoodViewPath);
        _neighborhoodView.Setup(_viewModel.NeighborhoodVm);
    }
    #endregion

    #region Rule type radio buttons
    private void SetupRuleTypeButtons()
    {
        SetupTotalisticRadioButton();
        SetupNontotalisticRadioButton();
    }

    #region Totalistic radio button
    [Export] public NodePath TotalisticRadioButtonPath { get; set; } = null!;
    private RadioButton _totalisticRadioButton = null!;

    private void SetupTotalisticRadioButton()
    {
        _totalisticRadioButton = GetNode<RadioButton>(TotalisticRadioButtonPath);
        SetupRuleTypeButton(_totalisticRadioButton, UniversalAutomationRuleType.Totalistic);
}
    #endregion

    #region Nontotalistic radio button
    [Export] public NodePath NontotalisticRadioButtonPath { get; set; } = null!;
    private RadioButton _nontotalisticRadioButton = null!;

    private void SetupNontotalisticRadioButton()
    {
        _nontotalisticRadioButton = GetNode<RadioButton>(NontotalisticRadioButtonPath);
        SetupRuleTypeButton(_nontotalisticRadioButton, UniversalAutomationRuleType.Nontotalistic);
    }
    #endregion

    private void SetupRuleTypeButton(RadioButton ruleTypeButton, UniversalAutomationRuleType ruleType)
    {
        PropertyBinder.BindTwoWay(this, ruleTypeButton, t => t.IsSelected, _viewModel, s => s.RuleType,
            isSelected => isSelected ? ruleType : _viewModel.RuleType,
            newRuleType => newRuleType == ruleType);
    }
    #endregion

    //#region Neighbor counts view
    //[Export] public NodePath NeighborCountsViewPath { get; set; } = null!;
    //private UniversalAutomationRuleNeighborCountsView _neighborCountsView = null!;

    //private void SetupNeighborCountsView()
    //{
    //    _neighborCountsView = GetNode<UniversalAutomationRuleNeighborCountsView>(NeighborCountsViewPath);
    //    _neighborCountsView.Setup(_viewModel.NeighborCountsVM);
    //}
    //#endregion

    //#region Neighbor positions view
    //[Export] public NodePath NeighborPositionsViewPath { get; set; } = null!;
    //private UniversalAutomationRuleNeighborPositionsView _neighborPositionsView = null!;

    //private void SetupNeighborPositionsView()
    //{
    //    _neighborPositionsView = GetNode<UniversalAutomationRuleNeighborCountsView>(NeighborPositionsViewPath);
    //    _neighborPositionsView.Setup(_viewModel.NeighborPositionsVm);
    //}
    //#endregion

    #region Neighbor counts box
    [Export] public NodePath NeighborCountsBoxPath { get; set; } = null!;
    private Container _neighborCountsBox = null!;

    private readonly IDictionary<int, ToggleButton> _neighborButtonsByCount = new Dictionary<int, ToggleButton>();

    private void SetupNeighborCountsBox()
    {
        _neighborCountsBox = GetNode<Container>(NeighborCountsBoxPath);

        GenerateNeighborCountButtons();
        UpdateNeighborCountButtons();

        TriggerBinder.OnPropertyChanged(this, _viewModel.NeighborhoodVm, o => o.Radius, GenerateNeighborCountButtons);
        TriggerBinder.OnPropertyChanged(this, _viewModel.NeighborhoodVm, o => o.State, UpdateNeighborCountButtons);
    }

    private void GenerateNeighborCountButtons()
    {
        foreach (var (_, button) in _neighborButtonsByCount)
            TriggerBinder.UnbindPropertyChanged(this, button, o => o.IsToggled);

        _neighborButtonsByCount.Clear();
        _neighborCountsBox.RemoveChildren();

        UniversalAutomationNeighborhood.IterateOverNeighborCounts(_viewModel.NeighborhoodVm.Radius, neighborCount =>
        {
            var button = new ToggleButton()
            {
                Text = $"{neighborCount}",
                CustomMinimumSize = new(28, 28),
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                FocusMode = FocusModeEnum.None,
                IsToggled = _viewModel.NeighborCounts.Contains(neighborCount)
            };
            button.AddThemeFontSizeOverride("font_size", 12);
            
            _neighborCountsBox.AddChild(button);

            _neighborButtonsByCount[neighborCount] = button;
            TriggerBinder.OnPropertyChanged(this, button, o => o.IsToggled, OnNeighborCountButtonIsToggledChanged);
        });
    }

    private void OnNeighborCountButtonIsToggledChanged(ToggleButton button, bool isToggled)
    {
        var neighborCount = _neighborButtonsByCount.First(kv => kv.Value == button).Key;
        
        if (isToggled)
            _viewModel.NeighborCounts.Add(neighborCount);
        else
            _viewModel.NeighborCounts.Remove(neighborCount);

        UpdateButtonSelfModulate(button);
    }

    private void UpdateNeighborCountButtons()
    {
        var maxNeighborCount = _viewModel.NeighborhoodVm.SelectedPositions.Count;
        foreach (var (neighborCount, button) in _neighborButtonsByCount)
        {
            var isAllowedNeighborCount = neighborCount <= maxNeighborCount;
            if (!isAllowedNeighborCount)
            {
                _viewModel.NeighborCounts.Remove(neighborCount);
                button.SetPressedNoSignal(false);
            }

            button.IsEnabled = isAllowedNeighborCount;

            UpdateButtonSelfModulate(button);
        }
    }
    #endregion

    #region Neighbor positions grid
    [Export] public NodePath NeighborPositionsGridPath { get; set; } = null!;
    private GridContainer _neighborPositionsGrid = null!;

    private readonly IDictionary<Vector2I, ToggleButton> _neighborButtonsByPosition = new Dictionary<Vector2I, ToggleButton>();

    private void SetupNeighborPositionsGrid()
    {
        _neighborPositionsGrid = GetNode<GridContainer>(NeighborPositionsGridPath);

        TriggerBinder.OnPropertyChanged(this, _viewModel.NeighborhoodVm, o => o.State, GenerateNeighborPositionButtons);

        GenerateNeighborPositionButtons();
    }

    private void GenerateNeighborPositionButtons()
    {
        _neighborButtonsByPosition.Clear();
        _neighborPositionsGrid.RemoveChildren();

        var radius = _viewModel.NeighborhoodVm.Radius;
        var selectedPositions = _viewModel.NeighborhoodVm.SelectedPositions;

        _neighborPositionsGrid.Columns = UniversalAutomationNeighborhood.GetSide(radius);

        UniversalAutomationNeighborhood.IterateOverAllPositions(radius, (position, isCenter) =>
        {
            var button = new ToggleButton
            {
                CustomMinimumSize = new(28, 28),
                FocusMode = FocusModeEnum.None
            };

            _neighborPositionsGrid.AddChild(button);

            if (!selectedPositions.Contains(position) || isCenter)
            {
                button.IsEnabled = false;
                return;
            }

            button.Text = "*";

            _neighborButtonsByPosition[position] = button;
        });
    }
    #endregion

    private static void UpdateButtonSelfModulate(BaseButton button) => button.ToggleSelfModulate(Colors.Green, Colors.White);

    //#region Select all neighbor counts button
    //[Export] public NodePath SelectAllNeighborCountsButtonPath { get; set; } = null!;
    //private RichButton _selectAllNeighborCountsButton = null!;

    //private void SetupSelectAllNeighborCountsButton()
    //{
    //    _selectAllNeighborCountsButton = GetNode<RichButton>(SelectAllNeighborCountsButtonPath);
    //    CommandBinder.Bind(this, _selectAllNeighborCountsButton, _viewModel.SelectAllNeighborCountsCommand);
    //}
    //#endregion

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _neighborhoodView.Unsubscribe();
        PropertyBinder.Unbind(this);
        TriggerBinder.Unbind(this);
        CommandBinder.Unbind(this);
    }
}