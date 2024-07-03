using EasyBindings;
using Godot;

public partial class UniversalAutomationStateView : CollapsiblePanelContainer, IView<UniversalAutomationStateVM>
{
	private UniversalAutomationStateVM _viewModel = null!;

	public void Setup(UniversalAutomationStateVM viewModel)
	{
		_viewModel = viewModel;

		SetupControls();
	}

	#region Controls
	private void SetupControls()
	{
		UpgradeCollapseToggleButton();
		SetupDeleteButton();
		SetupNameTextEdit();
		SetupColorPickerButton();
	}

	#region Collapse toggle button
	private void UpgradeCollapseToggleButton() =>
		PropertyBinder.BindOneWay(this, CollapseToggleButton, t => t.Text, _viewModel, s => s.Name, name => $" {name}");
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

	#region Name text edit
	[Export] public NodePath NameTextEditPath { get; set; } = null!;
	private RichTextEdit _nameTextEdit = null!;

	private void SetupNameTextEdit()
	{
		_nameTextEdit = GetNode<RichTextEdit>(NameTextEditPath);
		_nameTextEdit.Text = _viewModel.Name;

		PropertyBinder.BindOneWayToSource(this, _nameTextEdit, t => t.Text, _viewModel, s => s.Name);
	}
	#endregion

	#region Color picker button
	[Export] public NodePath ColorPickerButtonPath { get; set; } = null!;
	private RichColorPickerButton _colorPickerButton = null!;

	private void SetupColorPickerButton()
	{
		_colorPickerButton = GetNode<RichColorPickerButton>(ColorPickerButtonPath);
		_colorPickerButton.Color = _viewModel.Color;

		PropertyBinder.BindOneWayToSource(this, _colorPickerButton, t => t.SelectedColor, _viewModel, s => s.Color);
	}
	#endregion
	#endregion

	public override void Unsubscribe()
	{
		base.Unsubscribe();
		PropertyBinder.Unbind(this);
		CommandBinder.Unbind(this);
	}
}