using EasyBindings;
using Godot;
using System.Collections.Generic;
using System.Collections.Specialized;

public partial class UniversalAutomationRuleSetView : CollapsiblePanelContainer, IView<UniversalAutomationRuleSetVM>
{
	private UniversalAutomationRuleSetVM _viewModel = null!;

	public void Setup(UniversalAutomationRuleSetVM viewModel)
	{
		_viewModel = viewModel;

		SetupControls();
	}

	#region Controls
	private void SetupControls()
    {
        SetupTopPanelControls();
		SetupApplicationsNumberBox();
		SetupRulesBox();
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

	#region Applications number box
    private void SetupApplicationsNumberBox()
    {
        SetupApplicationsNumberLabel();
        SetupApplicationsNumberSlider();
    }

	#region Applications number label
	[Export] public NodePath ApplicationsNumberLabelPath { get; set; } = null!;
	private Label _applicationsNumberLabel = null!;

	private void SetupApplicationsNumberLabel()
	{
		_applicationsNumberLabel = GetNode<Label>(ApplicationsNumberLabelPath);

		PropertyBinder.BindOneWay(this, _applicationsNumberLabel, t => t.Text, _viewModel, s => s.ApplicationsNumber,
			n => $"{n.ToStringWithDelimiter(3, ' ')} раз");
	}
	#endregion

	#region Applications number slider
	[Export] public NodePath ApplicationsNumberSliderPath { get; set; } = null!;
	private RichHSlider _applicationsNumberSlider = null!;

	private void SetupApplicationsNumberSlider()
	{
		_applicationsNumberSlider = GetNode<RichHSlider>(ApplicationsNumberSliderPath);
		_applicationsNumberSlider.MaxValue = UniversalAutomationSettings.MaxRuleSetApplicationsNumber;
		_applicationsNumberSlider.IntValue = _viewModel.ApplicationsNumber;

		PropertyBinder.BindOneWayToSource(this, _applicationsNumberSlider, t => t.IntValue, _viewModel, s => s.ApplicationsNumber);
	}
	#endregion
	#endregion

	#region Rules box
    private void SetupRulesBox()
    {
        SetupRulesViewsBox();
        SetupAddRuleButton();
    }

	#region Rule views box
	[Export] public NodePath RuleViewsBoxPath { get; set; } = null!;
	private Container _ruleViewsBox = null!;

	private readonly IDictionary<UniversalAutomationRuleVM, UniversalAutomationRuleView> _ruleViewsByVm =
		new Dictionary<UniversalAutomationRuleVM, UniversalAutomationRuleView>();

	private void SetupRulesViewsBox()
	{
		_ruleViewsBox = GetNode<Container>(RuleViewsBoxPath);

		foreach (var ruleVm in _viewModel.RuleVms)
			AddRuleView(ruleVm);

		TriggerBinder.OnCollectionChanged(this, _viewModel.RuleVms, (_, e) =>
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Move:
					RearrangeRuleViews();
					break;
				case NotifyCollectionChangedAction.Add:
					foreach (UniversalAutomationRuleVM ruleVm in e.NewItems!)
						AddRuleView(ruleVm);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (UniversalAutomationRuleVM ruleVm in e.OldItems!)
						RemoveRuleView(ruleVm);
					break;
			}
		});
	}

	private void RearrangeRuleViews()
	{
		foreach (var ruleView in _ruleViewsByVm.Values)
			_ruleViewsBox.RemoveChild(ruleView);

		foreach (var ruleVm in _viewModel.RuleVms)
			_ruleViewsBox.AddChild(_ruleViewsByVm[ruleVm]);
	}

	private void AddRuleView(UniversalAutomationRuleVM ruleVm)
	{
		var ruleView = UniversalAutomationSettings.RuleView.Instantiate<UniversalAutomationRuleView>();
		_ruleViewsByVm[ruleVm] = ruleView;

		_ruleViewsBox.AddChild(ruleView);
		ruleView.Setup(ruleVm);
	}

	private void RemoveRuleView(UniversalAutomationRuleVM ruleVm)
	{
		var ruleView = _ruleViewsByVm[ruleVm];
		_ruleViewsByVm.Remove(ruleVm);
		ruleView.Unsubscribe();

		_ruleViewsBox.RemoveChild(ruleView);
		ruleView.QueueFree();
	}
	#endregion

	#region Add rule button
	[Export] public NodePath AddRuleButtonPath { get; set; } = null!;
	private RichButton _addRuleButton = null!;

	private void SetupAddRuleButton()
	{
		_addRuleButton = GetNode<RichButton>(AddRuleButtonPath);

		CommandBinder.Bind(this, _addRuleButton, _viewModel.AddRuleCommand);
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