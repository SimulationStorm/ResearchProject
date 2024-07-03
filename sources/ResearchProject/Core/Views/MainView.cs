using EasyBindings.Interfaces;
using Godot;

// TODO: What if MainVM() will be injected?
public partial class MainView : Node, IUnsubscribe
{
	private MainVM _viewModel = null!;

	public override void _Ready()
	{
		_viewModel = new MainVM();

		SetupViews();
	}

	#region Views
	private void SetupViews()
	{
		SetupTopPanelView();
		SetupBasicInfoPanelView();
		SetupControlPanelView();
		SetupHelpPanelView();
		SetupSettingsPanelView();
        SetupSimulationManagerView();
    }

    #region Top panel
    [Export] public NodePath TopPanelViewPath { get; set; } = null!;
	private TopPanelView _topPanelView = null!;

	private void SetupTopPanelView()
	{
		_topPanelView = GetNode<TopPanelView>(TopPanelViewPath);
		_topPanelView.Setup(_viewModel.TopPanelVM);
	}
	#endregion

	#region Control panel 
	[Export] public NodePath ControlPanelViewPath { get; set; } = null!;
	private ControlPanelView _controlPanelView = null!;

	private void SetupControlPanelView()
	{
		_controlPanelView = GetNode<ControlPanelView>(ControlPanelViewPath);
		_controlPanelView.Setup(_viewModel.ControlPanelVM);
	}
	#endregion

	#region Basic info panel 
	[Export] public NodePath BasicInfoPanelViewPath { get; set; } = null!;
	private BasicInfoPanelView _basicInfoPanelView = null!;

	private void SetupBasicInfoPanelView()
	{
		_basicInfoPanelView = GetNode<BasicInfoPanelView>(BasicInfoPanelViewPath);
		_basicInfoPanelView.Setup(_viewModel.BasicInfoPanelVM);
	}
	#endregion

	#region Help panel 
	[Export] public NodePath HelpPanelViewPath { get; set; } = null!;
	private HelpPanelView _helpPanelView = null!;

	private void SetupHelpPanelView()
	{
		_helpPanelView = GetNode<HelpPanelView>(HelpPanelViewPath);
		_helpPanelView.Setup(_viewModel.HelpPanelVM);
	}
	#endregion

	#region Settings panel view
    [Export] public NodePath SettingsPanelViewPath { get; set; } = null!;
	private SettingsPanelView _settingsPanelView = null!;

    private void SetupSettingsPanelView()
    {
        _settingsPanelView = GetNode<SettingsPanelView>(SettingsPanelViewPath);
        _settingsPanelView.Setup(_viewModel.SettingsPanelVM);
    }
    #endregion

	#region Simulation manager 
	[Export] public NodePath SimulationManagerViewPath { get; set; } = null!;
	private SimulationManagerView _simulationManagerView = null!;

	private void SetupSimulationManagerView()
	{
		_simulationManagerView = GetNode<SimulationManagerView>(SimulationManagerViewPath);
		_simulationManagerView.Setup(_viewModel.SimulationManagerVM);
	}
	#endregion
	#endregion

	public void Unsubscribe()
	{
		_topPanelView.Unsubscribe();
		_basicInfoPanelView.Unsubscribe();
		_controlPanelView.Unsubscribe();
		_helpPanelView.Unsubscribe();
		_settingsPanelView.Unsubscribe();
		_simulationManagerView.Unsubscribe();
	}
}