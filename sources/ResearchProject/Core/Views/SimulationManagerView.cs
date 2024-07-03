using EasyBindings;
using Godot;

public partial class SimulationManagerView : Node, IView<SimulationManagerVM>
{
	private SimulationManagerVM _viewModel = null!;

	public void Setup(SimulationManagerVM viewModel)
	{
		_viewModel = viewModel;

		SwitchSimulationView(_viewModel.SimulationMode);

		TriggerBinder.OnPropertyChanged(this, _viewModel, o => o.SimulationMode, SwitchSimulationView);
	}

	private SimulationView? _currentSimulationView;

    private void SwitchSimulationView(SimulationMode simulationMode)
	{
		if (_currentSimulationView is not null)
		{
			_currentSimulationView.Unsubscribe();
			
			RemoveChild(_currentSimulationView);
			_currentSimulationView.QueueFree();
		}
		
        var viewScene = SimulationSettings.SimulationViewScenesByMode[simulationMode];
        _currentSimulationView = (SimulationView)viewScene.Instantiate();
		AddChild(_currentSimulationView);

        ((dynamic)_currentSimulationView).Setup((dynamic)_viewModel.CurrentSimulationVM!);
	}

    public void Unsubscribe()
    {
        _currentSimulationView?.Unsubscribe();
        TriggerBinder.Unbind(this);
    }
}