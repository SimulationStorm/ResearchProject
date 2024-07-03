using Godot;

public partial class LifeLikeAutomationView : SimulationView, IView<LifeLikeAutomationVM>
{
	private LifeLikeAutomationVM _viewModel = null!;

	public void Setup(LifeLikeAutomationVM viewModel)
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
	private LifeLikeAutomationFieldView _fieldView = null!;

	private void SetupFieldView()
	{
		_fieldView = GetNode<LifeLikeAutomationFieldView>(FieldViewPath);
		_fieldView.Setup(_viewModel.FieldVM);
	}
	#endregion

	#region Field UI view 
	[Export] public NodePath FieldUiViewPath { get; set; } = null!;
	private LifeLikeAutomationFieldUiView _fieldUiView = null!;

	private void SetupFieldUiView()
	{
		_fieldUiView = GetNode<LifeLikeAutomationFieldUiView>(FieldUiViewPath);
		_fieldUiView.Setup(_viewModel.FieldUiVM);
	}
	#endregion

	#region Menu view 
	[Export] public NodePath MenuViewPath { get; set; } = null!;
	private LifeLikeAutomationMenuView _menuView = null!;

	private void SetupMenuView()
	{
		_menuView = GetNode<LifeLikeAutomationMenuView>(MenuViewPath);
		_menuView.Setup(_viewModel.MenuVM);
	}
	#endregion

	#region Stats panel view 
	[Export] public NodePath StatsPanelViewPath { get; set; } = null!;
	private LifeLikeAutomationStatsPanelView _statsPanelView = null!;

	private void SetupStatsPanelView()
	{
		_statsPanelView = GetNode<LifeLikeAutomationStatsPanelView>(StatsPanelViewPath);
		_statsPanelView.Setup(_viewModel.StatsPanelVM);
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