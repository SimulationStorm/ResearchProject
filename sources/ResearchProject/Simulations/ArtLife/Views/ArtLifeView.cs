using Godot;

public partial class ArtLifeView : SimulationView
{
	private ArtLifeVM _viewModel = null!;

	public void Setup(ArtLifeVM viewModel)
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
		SetupWorldEnvPanelView();
		SetupCellInfoPanelView();
	}

	#region Field view 
	[Export] public NodePath FieldViewPath { get; set; } = null!;
	private ArtLifeFieldView _fieldView = null!;

	private void SetupFieldView()
	{
		_fieldView = GetNode<ArtLifeFieldView>(FieldViewPath);
		_fieldView.Setup(_viewModel.FieldVM);
	}
	#endregion

	#region Field UI view 
	[Export] public NodePath FieldUiViewPath { get; set; } = null!;
	private ArtLifeFieldUiView _fieldUiView = null!;

	private void SetupFieldUiView()
	{
		_fieldUiView = GetNode<ArtLifeFieldUiView>(FieldUiViewPath);
		_fieldUiView.Setup(_viewModel.FieldUiVM);
	}
	#endregion

	#region Menu view 
	[Export] public NodePath MenuViewPath { get; set; } = null!;
	private ArtLifeMenuView _menuView = null!;

	private void SetupMenuView()
	{
		_menuView = GetNode<ArtLifeMenuView>(MenuViewPath);
		_menuView.Setup(_viewModel.MenuVM);
	}
	#endregion

	#region Stats panel view 
	[Export] public NodePath StatsPanelViewPath { get; set; } = null!;
	private ArtLifeStatsPanelView _statsPanelView = null!;

	private void SetupStatsPanelView()
	{
		_statsPanelView = GetNode<ArtLifeStatsPanelView>(StatsPanelViewPath);
		_statsPanelView.Setup(_viewModel.StatsPanelVM);
	}
	#endregion

	#region World env panel view 
	[Export] public NodePath WorldEnvPanelViewPath { get; set; } = null!;
	private WorldEnvPanelView _worldEnvPanelView = null!;

	private void SetupWorldEnvPanelView()
	{
		_worldEnvPanelView = GetNode<WorldEnvPanelView>(WorldEnvPanelViewPath);
		_worldEnvPanelView.Setup(_viewModel.WorldEnvPanelVM);
	}
	#endregion

	#region Cell info panel view 
	[Export] public NodePath CellInfoPanelViewPath { get; set; } = null!;
	private CellInfoPanelView _cellInfoPanelView = null!;

	private void SetupCellInfoPanelView()
	{
		_cellInfoPanelView = GetNode<CellInfoPanelView>(CellInfoPanelViewPath);
		_cellInfoPanelView.Setup(_viewModel.CellInfoPanelVM);
	}
	#endregion
	#endregion

	public override void Unsubscribe()
	{
		_fieldView.Unsubscribe();
		_fieldUiView.Unsubscribe();
		_menuView.Unsubscribe();
		_statsPanelView.Unsubscribe();
		_cellInfoPanelView.Unsubscribe();
	}
}
