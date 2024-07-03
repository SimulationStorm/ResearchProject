using EasyBindings;
using Godot;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

public partial class UniversalAutomationMenuView : AutomationMenuView<UniversalAutomationState>, IView<UniversalAutomationMenuVM>
{
	private UniversalAutomationMenuVM _viewModel = null!;

	public void Setup(UniversalAutomationMenuVM viewModel)
	{
		_viewModel = viewModel;
		base.Setup(viewModel);

		SetupControls();
	}

	#region Controls
	private void SetupControls()
	{
		SetupKindsBox();
		SetupStatesBox();
		SetupRuleSetsApplicationsNumberBox();
		SetupRuleSetsBox();
		SetupRandomSeedBox();
	}

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

        PropertyBinder.BindOneWay(this, _currentKindLabel, t => t.Text, _viewModel, s => s.Kind, kind => kind is not null ? $"\"{kind.Name}\"" : "не выбрана");
        PropertyBinder.BindOneWay(this, _currentKindLabel, t => t.SelfModulate, _viewModel, s => s.Kind, kind => kind is not null ? Colors.Green : Colors.Red);
    }
    #endregion

    #region Kind category tab container
    [Export] public NodePath KindCategoryTabContainerPath { get; set; } = null!;
    private TabContainer _kindCategoryTabContainer = null!;

    private void SetupKindCategoryTabContainer()
    {
        _kindCategoryTabContainer = GetNode<TabContainer>(KindCategoryTabContainerPath);

        var kindRadioButtonGroup = new RadioButtonGroup();

        foreach (var kindCategory in UniversalAutomationKindCategory.All)
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

                categoryBox.AddChild(kindRadioButton);
            }

            _kindCategoryTabContainer.AddChild(marginContainer);
        }

        PropertyBinder.BindTwoWay(this, kindRadioButtonGroup, t => t.SelectedButton, _viewModel, s => s.Kind,
            selectedButton =>
                selectedButton is not null ? UniversalAutomationKind.ByName(selectedButton.Text) : null,
            kind =>
                kind is null ? null : kindRadioButtonGroup.Buttons.First(b => b.Text == kind.Name));
    }
    #endregion
    #endregion

    #region States box
    private void SetupStatesBox()
    {
        SetupDefaultStateOptionButton();
        SetupStateViewsBox();
		SetupAddStateButton();
	}

	#region Default state option button
    [Export] public NodePath DefaultStateOptionButtonPath { get; set; } = null!;
    private RichOptionButton _defaultStateOptionButton = null!;

    private void SetupDefaultStateOptionButton()
    {
        _defaultStateOptionButton = GetNode<RichOptionButton>(DefaultStateOptionButtonPath);

        ResetItemsAndSelectedIndex();

        TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.States, ResetItemsAndSelectedIndex);

        _defaultStateOptionButton.ItemSelected += item =>
        {
            var itemIndex = _defaultStateOptionButton.Items.ToList().IndexOf(item);
            var state = _viewModel.States[itemIndex];
            _viewModel.DefaultState = state;
        };

        void ResetItemsAndSelectedIndex()
        {
            var prevSelectedIndex = _defaultStateOptionButton.SelectedIndex;

            _defaultStateOptionButton.Items = _viewModel.States.Select(state => new OptionItem(_viewModel.NamesByState[state]));

            var maxIndex = _viewModel.States.Count - 1;

            if (prevSelectedIndex is -1)
                _defaultStateOptionButton.SelectedIndex = _viewModel.States.IndexOf(_viewModel.DefaultState);
            else if (prevSelectedIndex <= maxIndex)
                _defaultStateOptionButton.SelectedIndex = prevSelectedIndex;
            else
                _defaultStateOptionButton.SelectedIndex = maxIndex;
        }

//      _defaultStateOptionButton.Items = _viewModel.StateNumbers.Select(state => new OptionItem($"{state.Name} ({state.Number})"));

        //      UpdateDefaultStateOptionButtonSelectedIndex();

        //      _defaultStateOptionButton.ItemSelected += item =>
        //      {
        //          var stateNumber = int.Parse(item.Text.Split(' ', StringSplitOptions.TrimEntries).Last());
        //          _viewModel.DefaultStateNumber = _viewModel.StateNumbers.First(state => state.Number == stateNumber);
        //      };

        //TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.DefaultStateNumber, UpdateDefaultStateOptionButtonSelectedIndex);
        //TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.StateNumbers, );
    }

    //private void UpdateDefaultStateOptionButtonSelectedIndex() =>
    //    _defaultStateOptionButton.SelectedIndex = _viewModel.StateNumbers.IndexOf(_viewModel.DefaultStateNumber);
    #endregion

    #region State views box
    [Export] public NodePath StateViewsBoxPath { get; set; } = null!;
	private Container _stateViewsBox = null!;

	private readonly IDictionary<UniversalAutomationStateVM, UniversalAutomationStateView> _stateViewsByVm =
		new Dictionary<UniversalAutomationStateVM, UniversalAutomationStateView>();

	private void SetupStateViewsBox()
	{
		_stateViewsBox = GetNode<Container>(StateViewsBoxPath);

		foreach (var stateVm in _viewModel.StateVms)
			AddStateView(stateVm);

		TriggerBinder.OnCollectionChanged(this, _viewModel.StateVms, e =>
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (UniversalAutomationStateVM stateVm in e.NewItems!)
						AddStateView(stateVm);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (UniversalAutomationStateVM stateVm in e.OldItems!)
						RemoveStateView(stateVm);
					break;
			}
		});
	}

	private void AddStateView(UniversalAutomationStateVM stateVm)
	{
		var stateView = UniversalAutomationSettings.StateView.Instantiate<UniversalAutomationStateView>();
		_stateViewsByVm[stateVm] = stateView;

		_stateViewsBox.AddChild(stateView);
		stateView.Setup(stateVm);
	}

	private void RemoveStateView(UniversalAutomationStateVM stateVm)
	{
		var stateView = _stateViewsByVm[stateVm];
		_stateViewsByVm.Remove(stateVm);
		stateView.Unsubscribe();

		_stateViewsBox.RemoveChild(stateView);
		stateView.QueueFree();
	}
	#endregion

	#region Add state button
	[Export] public NodePath AddStateButtonPath { get; set; } = null!;
	private RichButton _addStateButton = null!;

	private void SetupAddStateButton()
	{
		_addStateButton = GetNode<RichButton>(AddStateButtonPath);

		CommandBinder.Bind(this, _addStateButton, _viewModel.AddStateCommand);
	}
	#endregion
	#endregion

	#region Rule sets applications number box
	private void SetupRuleSetsApplicationsNumberBox()
	{
		SetupRuleSetsRepetitionsNumberLabel();
		SetupRuleSetsNumberOfRepetitionsSlider();
	}

	#region Rule sets repititions number label
	[Export] public NodePath RuleSetsRepetitionsNumberLabelPath { get; set; } = null!;
	private Label _ruleSetsRepetitionsNumberLabel = null!;

	private void SetupRuleSetsRepetitionsNumberLabel()
	{
		_ruleSetsRepetitionsNumberLabel = GetNode<Label>(RuleSetsRepetitionsNumberLabelPath);

		PropertyBinder.BindOneWay(this, _ruleSetsRepetitionsNumberLabel, t => t.Text, _viewModel, s => s.RuleSetsRepetitionsNumber,
			n => $"{n.ToStringWithDelimiter(3, ' ')} раз");
	}
	#endregion

	#region Rule sets repititions number slider
	[Export] public NodePath RuleSetsRepetitionsNumberSliderPath { get; set; } = null!;
	private RichHSlider _ruleSetsRepetitionsNumberSlider = null!;

	private void SetupRuleSetsNumberOfRepetitionsSlider()
	{
		_ruleSetsRepetitionsNumberSlider = GetNode<RichHSlider>(RuleSetsRepetitionsNumberSliderPath);
		_ruleSetsRepetitionsNumberSlider.MaxValue = UniversalAutomationSettings.MaxRuleSetsRepetitionsNumber;
		_ruleSetsRepetitionsNumberSlider.IntValue = _viewModel.RuleSetsRepetitionsNumber;

		PropertyBinder.BindOneWayToSource(this, _ruleSetsRepetitionsNumberSlider, t => t.IntValue, _viewModel, s => s.RuleSetsRepetitionsNumber);
	}
	#endregion
	#endregion

	#region Rule sets box
	private void SetupRuleSetsBox()
	{
		SetupRuleSetViewsBox();
		SetupAddRuleSetButton();
	}
	
	#region Rule set views box
	[Export] public NodePath RuleSetViewsBoxPath { get; set; } = null!;
	private Container _ruleSetViewsBox = null!;

	private readonly IDictionary<UniversalAutomationRuleSetVM, UniversalAutomationRuleSetView> _ruleSetViewsByVm =
		new Dictionary<UniversalAutomationRuleSetVM, UniversalAutomationRuleSetView>();

	private void SetupRuleSetViewsBox()
	{
		_ruleSetViewsBox = GetNode<Container>(RuleSetViewsBoxPath);

		foreach (var ruleSetVm in _viewModel.RuleSetVms)
			AddRuleSetView(ruleSetVm);

		TriggerBinder.OnCollectionChanged(this, _viewModel.RuleSetVms, e =>
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Move:
					RearrangeRuleSetViews();
					break;
				case NotifyCollectionChangedAction.Add:
					foreach (UniversalAutomationRuleSetVM ruleSetVm in e.NewItems!)
						AddRuleSetView(ruleSetVm);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (UniversalAutomationRuleSetVM ruleSetVm in e.OldItems!)
						RemoveRuleSetView(ruleSetVm);
					break;
			}
		});
	}

	private void RearrangeRuleSetViews()
	{
		foreach (var ruleSetView in _ruleSetViewsByVm.Values)
			_ruleSetViewsBox.RemoveChild(ruleSetView);

		foreach (var ruleSetVm in _viewModel.RuleSetVms)
			_ruleSetViewsBox.AddChild(_ruleSetViewsByVm[ruleSetVm]);
	}

	private void AddRuleSetView(UniversalAutomationRuleSetVM ruleSetVm)
	{
		var ruleSetView = UniversalAutomationSettings.RuleSetView.Instantiate<UniversalAutomationRuleSetView>();
		_ruleSetViewsByVm[ruleSetVm] = ruleSetView;

		_ruleSetViewsBox.AddChild(ruleSetView);
		ruleSetView.Setup(ruleSetVm);
	}

	private void RemoveRuleSetView(UniversalAutomationRuleSetVM ruleSetVm)
	{
		var ruleSetView = _ruleSetViewsByVm[ruleSetVm];
		_ruleSetViewsByVm.Remove(ruleSetVm);
		ruleSetView.Unsubscribe();

		_ruleSetViewsBox.RemoveChild(ruleSetView);
		ruleSetView.QueueFree();
	}
	#endregion

	#region Add rule set button
	[Export] public NodePath AddRuleSetButtonPath { get; set; } = null!;
	private RichButton _addRuleSetButton = null!;

	private void SetupAddRuleSetButton()
	{
		_addRuleSetButton = GetNode<RichButton>(AddRuleSetButtonPath);

		CommandBinder.Bind(this, _addRuleSetButton, _viewModel.AddRuleSetCommand);
	}
	#endregion
	#endregion

	#region Random seed box
	private void SetupRandomSeedBox()
	{
		SetupRandomSeedSpinBox();
		SetupResetRandomSeedButton();
	}

	#region Random seed spin box
	[Export] public NodePath RandomSeedSpinBoxPath { get; set; } = null!;
	private RichSpinBox _randomSeedSpinBox = null!;

	private void SetupRandomSeedSpinBox()
	{
		_randomSeedSpinBox = GetNode<RichSpinBox>(RandomSeedSpinBoxPath);

		_randomSeedSpinBox.MinValue = 0;
		_randomSeedSpinBox.MaxValue = int.MaxValue;

		_randomSeedSpinBox.Value = _viewModel.RandomSeed;
		PropertyBinder.BindTwoWay(this, _randomSeedSpinBox, t => t.Value, _viewModel, s => s.RandomSeed,
			 seedDoubleValue => (int)seedDoubleValue,
			 seedIntValue => seedIntValue);
	}
	#endregion

	#region Reset random seed button
	[Export] public NodePath ResetRandomSeedButtonPath { get; set; } = null!;
	private RichButton _resetRandomSeedButton = null!;

	private void SetupResetRandomSeedButton()
	{
		_resetRandomSeedButton = GetNode<RichButton>(ResetRandomSeedButtonPath);

		CommandBinder.Bind(this, _resetRandomSeedButton, _viewModel.ResetRandomSeedCommand);
	}
	#endregion
	#endregion

    #region Drawing mode view
    private UniversalAutomationDrawingModeView _drawingModeView = null!;
    protected override void SetupDrawingModeView()
    {
        _drawingModeView = GetNode<UniversalAutomationDrawingModeView>(DrawingModeViewPath);
        _drawingModeView.Setup(_viewModel.DrawingModeVm);
    }
    #endregion

    #region Field wrapping view
    private UniversalAutomationFieldWrappingView _fieldWrappingView = null!;
    protected override void SetupFieldWrappingView()
    {
        _fieldWrappingView = GetNode<UniversalAutomationFieldWrappingView>(FieldWrappingViewPath);
        _fieldWrappingView.Setup(_viewModel.FieldWrappingVm);
    }
    #endregion
    #endregion

	public override void Unsubscribe()
	{
		base.Unsubscribe();
		TriggerBinder.Unbind(this);
		CommandBinder.Unbind(this);
	}
}