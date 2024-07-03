using Godot;

public partial class UniversalAutomationView : SimulationView, IView<UniversalAutomationVM>
{
	private UniversalAutomationVM _viewModel = null!;

	public void Setup(UniversalAutomationVM viewModel)
	{
		_viewModel = viewModel;
		SetupViews();
	}

	#region Views
	private void SetupViews()
	{
		SetupFieldView();
		SetupFieldUiView();
		SetupMenuView();
		SetupStatsPanelView();
	}

	#region Field view 
	[Export] public NodePath FieldViewPath { get; set; } = null!;
	private UniversalAutomationFieldView _fieldView = null!;

	private void SetupFieldView()
	{
		_fieldView = GetNode<UniversalAutomationFieldView>(FieldViewPath);
		_fieldView.Setup(_viewModel.FieldVm);
	}
	#endregion

	#region Field UI view 
	[Export] public NodePath FieldUiViewPath { get; set; } = null!;
	private UniversalAutomationFieldUiView _fieldUiView = null!;

	private void SetupFieldUiView()
	{
		_fieldUiView = GetNode<UniversalAutomationFieldUiView>(FieldUiViewPath);
		_fieldUiView.Setup(_viewModel.FieldUiVm);
	}
	#endregion

	#region Menu view 
	[Export] public NodePath MenuViewPath { get; set; } = null!;
	private UniversalAutomationMenuView _menuView = null!;

	private void SetupMenuView()
	{
		_menuView = GetNode<UniversalAutomationMenuView>(MenuViewPath);
		_menuView.Setup(_viewModel.MenuVm);
	}
	#endregion

	#region Stats panel view 
	[Export] public NodePath StatsPanelViewPath { get; set; } = null!;
	private UniversalAutomationStatsPanelView _statsPanelView = null!;

	private void SetupStatsPanelView()
	{
		_statsPanelView = GetNode<UniversalAutomationStatsPanelView>(StatsPanelViewPath);
		_statsPanelView.Setup(_viewModel.StatsPanelVm);
	}
	#endregion
	#endregion

	public override void Unsubscribe()
	{
		_fieldView.Unsubscribe();
		_fieldUiView.Unsubscribe();
		_menuView.Unsubscribe();
		_statsPanelView.Unsubscribe();
	}
}