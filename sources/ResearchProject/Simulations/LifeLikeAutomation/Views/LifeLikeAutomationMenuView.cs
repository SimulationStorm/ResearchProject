using System;
using System.ComponentModel;
using EasyBindings;
using Godot;
using System.Linq;

public partial class LifeLikeAutomationMenuView : AutomationMenuView<LifeLikeAutomationCellState>, IView<LifeLikeAutomationMenuVM>
{
    private LifeLikeAutomationMenuVM _viewModel = null!;

    public void Setup(LifeLikeAutomationMenuVM viewModel)
    {
        _viewModel = viewModel;
        base.Setup(viewModel);

        SetupControls();
    }

    #region Controls
    private void SetupControls()
    {
        SetupRuleBox();
        SetupKindsBox();
        SetupPatternsBox();
        SetupPopulationBox();
        SetupCellColorsBox();
        SetupAlgorithmsBox();
    }

    #region Rule box
    private void SetupRuleBox()
    {
        SetupCurrentRuleLabel();
        SetupRuleView();
        SetupResetRuleButton();
    }

    #region Current rule label
    [Export] public NodePath CurrentRuleLabelPath { get; set; } = null!;
    private Label _currentRuleLabel = null!;

    private void SetupCurrentRuleLabel()
    {
        _currentRuleLabel = GetNode<Label>(CurrentRuleLabelPath);

        TriggerBinder.OnPropertyChanged(this, _viewModel.RuleVM, s => s.State, () =>
        {
            _currentRuleLabel.Text = _viewModel.RuleVM.AsRule().ToString();
            _currentRuleLabel.ToggleSelfModulate(_viewModel.RuleVM.AsRule().Equals(LifeLikeAutomationRule.Empty), Colors.Red, Colors.Green);
        });
    }
    #endregion

    #region Rule view
    [Export] public NodePath RuleViewPath { get; set; } = null!;
    private LifeLikeAutomationRuleView _ruleView = null!;

    private void SetupRuleView()
    {
        _ruleView = GetNode<LifeLikeAutomationRuleView>(RuleViewPath);
        _ruleView.Setup(_viewModel.RuleVM);
    }
    #endregion

    #region Reset rule button
    [Export] NodePath ResetRuleButtonPath { get; set; } = null!;
    private RichButton _resetRuleButton = null!;

    private void SetupResetRuleButton()
    {
        _resetRuleButton = GetNode<RichButton>(ResetRuleButtonPath);

        CommandBinder.Bind(this, _resetRuleButton, _viewModel.ResetRuleCommand);
    }
    #endregion
    #endregion

    #region Kinds box
    private void SetupKindsBox()
    {
        SetupCurrentKindLabel();
        SetupKindCategoryTabContainer();
    }

    #region Current kind label
    [Export] public NodePath CurrentKindLabelPath { get; set; } = null!;
    private Label _currentKindLabel = null!;

    private void SetupCurrentKindLabel()
    {
        _currentKindLabel = GetNode<Label>(CurrentKindLabelPath);

        PropertyBinder.BindOneWay(this, _currentKindLabel, t => t.Text, _viewModel, s => s.Kind, kind =>
            kind is not null ? kind.Name : "не выбрана");

        PropertyBinder.BindOneWay(this, _currentKindLabel, t => t.SelfModulate, _viewModel, s => s.Kind, kind =>
            kind is not null ? Colors.Green : Colors.Red);
    }
    #endregion

    #region Kind category tab container
    [Export] public NodePath KindCategoryTabContainerPath { get; set; } = null!;
    private TabContainer _kindCategoryTabContainer = null!;

    private void SetupKindCategoryTabContainer()
    {
        _kindCategoryTabContainer = GetNode<TabContainer>(KindCategoryTabContainerPath);

        var kindRadioButtonGroup = new RadioButtonGroup();

        foreach (var kindCategory in LifeLikeAutomationKindCategory.All)
        {
            var marginContainer = new MarginContainer
            {
                Name = kindCategory.Name
            };
            marginContainer.AddThemeConstantOverride("margin_left", 10);
            marginContainer.AddThemeConstantOverride("margin_top", 10);
            marginContainer.AddThemeConstantOverride("margin_right", 10);
            marginContainer.AddThemeConstantOverride("margin_bottom", 10);

            var categoryBox = new VBoxContainer();
            categoryBox.AddThemeConstantOverride("separation", 10);
            
            marginContainer.AddChild(categoryBox);

            var descriptionBox = new VBoxContainer();
            descriptionBox.AddChild(new Label
            {
                Text = kindCategory.Description,
                VerticalAlignment = VerticalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.Word,
                LabelSettings = new LabelSettings { FontSize = 12 }
            });
            descriptionBox.AddChild(new HSeparator());
            categoryBox.AddChild(descriptionBox);

            foreach (var kind in kindCategory.Kinds)
            {
                var kindRadioButton = new RadioButton
                {
                    Text = kind.Name,
                    ToggleMode = true,
                    SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
                    FocusMode = FocusModeEnum.None
                };
                kindRadioButtonGroup.Add(kindRadioButton);

                var kindRuleLabel = new Label
                {
                    Text = $"({kind.Rule})",
                    SizeFlagsHorizontal = SizeFlags.ExpandFill,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                var boxContainer = new HBoxContainer();
                boxContainer.AddChild(kindRadioButton);
                boxContainer.AddChild(kindRuleLabel);

                categoryBox.AddChild(boxContainer);
            }

            _kindCategoryTabContainer.AddChild(marginContainer);
        }

        PropertyBinder.BindTwoWay(this, kindRadioButtonGroup, t => t.SelectedButton, _viewModel, s => s.Kind,
            selectedButton =>
                selectedButton is not null ? LifeLikeAutomationKind.ByName(selectedButton.Text) : null,
            kind =>
                kind is null ? null : kindRadioButtonGroup.Buttons.First(b => b.Text == kind.Name));
    }
    #endregion
    #endregion

    #region Patterns box
    private void SetupPatternsBox()
    {
        SetupCurrentPatternLabel();
        SetupPatternCategoryTabContainer();
        SetupUnselectPatternButton();
    }

    #region Current pattern label
    [Export] public NodePath CurrentPatternLabelPath { get; set; } = null!;
    private Label _currentPatternLabel = null!;

    private void SetupCurrentPatternLabel()
    {
        _currentPatternLabel = GetNode<Label>(CurrentPatternLabelPath);

        PropertyBinder.BindOneWay(this, _currentPatternLabel, t => t.Text, _viewModel, s => s.Pattern, pattern =>
            pattern is not null ? pattern.Name : "не выбран");

        PropertyBinder.BindOneWay(this, _currentPatternLabel, t => t.SelfModulate, _viewModel, s => s.Kind, kind =>
            kind is not null ? Colors.Green : Colors.Red);
    }
    #endregion

    #region Pattern category tab container
    [Export] public NodePath PatternCategoryTabContainerPath { get; set; } = null!;
    private TabContainer _patternCategoryTabContainer = null!;

    private void SetupPatternCategoryTabContainer()
    {
        _patternCategoryTabContainer = GetNode<TabContainer>(PatternCategoryTabContainerPath);

        var patternRadioButtonGroup = new RadioButtonGroup();

        foreach (var patternCategory in LifeLikeAutomationPatternCategory.All)
        {
            var marginContainer = new MarginContainer
            {
                Name = patternCategory.Name
            };
            marginContainer.AddThemeConstantOverride("margin_left", 10);
            marginContainer.AddThemeConstantOverride("margin_top", 10);
            marginContainer.AddThemeConstantOverride("margin_right", 10);
            marginContainer.AddThemeConstantOverride("margin_bottom", 10);

            var categoryBox = new VBoxContainer();
            categoryBox.AddThemeConstantOverride("separation", 10);

            marginContainer.AddChild(categoryBox);

            var descriptionBox = new VBoxContainer();
            descriptionBox.AddChild(new Label
            {
                Text = patternCategory.Description,
                VerticalAlignment = VerticalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.Word,
                LabelSettings = new LabelSettings { FontSize = 12 }
            });
            descriptionBox.AddChild(new HSeparator());
            categoryBox.AddChild(descriptionBox);

            foreach (var pattern in patternCategory.Patterns)
            {
                var patternRadioButton = new RadioButton
                {
                    Text = pattern.Name,
                    ToggleMode = true,
                    SizeFlagsHorizontal = SizeFlags.ShrinkBegin,
                    FocusMode = FocusModeEnum.None
                };

                patternRadioButtonGroup.Add(patternRadioButton);
                categoryBox.AddChild(patternRadioButton);
            }

            _patternCategoryTabContainer.AddChild(marginContainer);
        }

        PropertyBinder.BindTwoWay(this, patternRadioButtonGroup, t => t.SelectedButton, _viewModel, s => s.Pattern,
            selectedButton =>
                selectedButton is not null ? LifeLikeAutomationPattern.ByName(selectedButton.Text) : null,
            pattern =>
                pattern is null ? null : patternRadioButtonGroup.Buttons.First(b => b.Text == pattern.Name));
    }
    #endregion

    #region Unselect pattern button
    [Export] public NodePath UnselectPatternButtonPath { get; set; } = null!;
    private RichButton _unselectPatternButton = null!;

    private void SetupUnselectPatternButton()
    {
        _unselectPatternButton = GetNode<RichButton>(UnselectPatternButtonPath);

        CommandBinder.Bind(this, _unselectPatternButton, _viewModel.UnselectPatternCommand);
    }
    #endregion
    #endregion

    #region Population box
    private void SetupPopulationBox()
    {
        SetupLiveDensityLabel();
        SetupLiveDensitySlider();
        SetupPopulateButton();
    }

    #region  Live density label
    [Export] public NodePath LiveDensityLabelPath { get; set; } = null!;
    private Label _liveDensityLabel = null!;

    private void SetupLiveDensityLabel()
    {
        _liveDensityLabel = GetNode<Label>(LiveDensityLabelPath);

        PropertyBinder.BindOneWay(this, _liveDensityLabel, t => t.Text, _viewModel, s => s.LiveDensity, Converters.PercentToString);
        PropertyBinder.BindOneWay(this, _liveDensityLabel, t => t.SelfModulate, _viewModel, s => s.LiveDensity, Converters.PercentToColor);
    }
    #endregion

    #region Live density slider
    [Export] public NodePath LiveDensitySliderPath { get; set; } = null!;
    private RichHSlider _liveDensitySlider = null!;

    private void SetupLiveDensitySlider()
    {
        _liveDensitySlider = GetNode<RichHSlider>(LiveDensitySliderPath);
        _liveDensitySlider.MinValue = LifeLikeAutomationSettings.MinLiveDensity;
        _liveDensitySlider.MaxValue = LifeLikeAutomationSettings.MaxLiveDensity;
        _liveDensitySlider.Step = LifeLikeAutomationSettings.LiveDensityStep;
        _liveDensitySlider.Value = _viewModel.LiveDensity;

        PropertyBinder.BindOneWayToSource(this, _liveDensitySlider, t => t.Value, _viewModel, s => s.LiveDensity);
    }
    #endregion

    #region Populate button
    [Export] public NodePath PopulateButtonPath { get; set; } = null!;
    private RichButton _populateButton = null!;

    private void SetupPopulateButton()
    {
        _populateButton = GetNode<RichButton>(PopulateButtonPath);

        CommandBinder.Bind(this, _populateButton, _viewModel.PopulateCommand);
    }
    #endregion
    #endregion

    #region Cell colors box
    private void SetupCellColorsBox()
    {
        SetupCellColorPickerButtons();
        SetupChooseRandomCellColorsButton();
    }

    #region Cell color picker buttons
    [Export] public NodePath DeadCellColorPickerButtonPath { get; set; } = null!;
    private RichColorPickerButton _deadCellColorPickerButton = null!;

    [Export] public NodePath AliveCellColorPickerButtonPath { get; set; } = null!;
    private RichColorPickerButton _aliveCellColorPickerButton = null!;

    private void SetupCellColorPickerButtons()
    {
        _deadCellColorPickerButton = GetNode<RichColorPickerButton>(DeadCellColorPickerButtonPath);
        _aliveCellColorPickerButton = GetNode<RichColorPickerButton>(AliveCellColorPickerButtonPath);

        PropertyBinder.BindTwoWay(this, _deadCellColorPickerButton, t => t.SelectedColor, _viewModel, s => s.DeadCellColor);
        PropertyBinder.BindTwoWay(this, _aliveCellColorPickerButton, t => t.SelectedColor, _viewModel, s => s.AliveCellColor);
    }
    #endregion

    #region Choose random cell colors button
    [Export] public NodePath ChooseRandomCellColorsButtonPath { get; set; } = null!;
    private RichButton _chooseRandomCellColorsButton = null!;

    private void SetupChooseRandomCellColorsButton()
    {
        _chooseRandomCellColorsButton = GetNode<RichButton>(ChooseRandomCellColorsButtonPath);

        CommandBinder.Bind(this, _chooseRandomCellColorsButton, _viewModel.ChooseRandomCellColorsCommand);
    }
    #endregion
    #endregion

    #region Algorithms box
    [Export] public NodePath AlgorithmsBoxPath { get; set; } = null!;
    private BoxContainer _algorithmsBox = null!;

    private void SetupAlgorithmsBox()
    {
        _algorithmsBox = GetNode<BoxContainer>(AlgorithmsBoxPath);

        var radioButtonGroup = new RadioButtonGroup();
        foreach (var algorithm in LifeLikeAutomationAlgorithm.All)
        {
            var radioButton = new RadioButton
            {
                Text = algorithm.Name,
                FocusMode = FocusModeEnum.None,
                IsSelected = algorithm == _viewModel.Algorithm
            };

            radioButtonGroup.Add(radioButton);

            _algorithmsBox.AddChild(radioButton);
        }

        PropertyBinder.BindOneWayToSource(this, radioButtonGroup, t => t.SelectedButton,
            _viewModel, s => s.Algorithm, selectedButton => LifeLikeAutomationAlgorithm.ByName(selectedButton!.Text));
    }
    #endregion

    #region Drawing mode view
    private LifeLikeAutomationDrawingModeView _drawingModeView = null!;
    protected override void SetupDrawingModeView()
    {
        _drawingModeView = GetNode<LifeLikeAutomationDrawingModeView>(DrawingModeViewPath);
        _drawingModeView.Setup(_viewModel.DrawingModeVM);
    }
    #endregion

    #region Field wrapping view
    private LifeLikeAutomationFieldWrappingView _fieldWrappingView = null!;
    protected override void SetupFieldWrappingView()
    {
        _fieldWrappingView = GetNode<LifeLikeAutomationFieldWrappingView>(FieldWrappingViewPath);
        _fieldWrappingView.Setup(_viewModel.FieldWrappingVM);
    }
    #endregion
    #endregion

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _drawingModeView.Unsubscribe();
        _fieldWrappingView.Unsubscribe();
        _ruleView.Unsubscribe();
        PropertyBinder.Unbind(this);
        TriggerBinder.Unbind(this);
        CommandBinder.Unbind(this);
    }
}