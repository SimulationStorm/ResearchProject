using Godot;

public partial class ArtLifeFieldUiView : SimulationFieldUiView
{
	private ArtLifeFieldUiVM _viewModel = null!;

	public void Setup(ArtLifeFieldUiVM viewModel)
	{
		base.Setup(viewModel);
		_viewModel = viewModel;
	}

	protected override void OnCellPressed(Vector2I cell, MouseButton mouseButton)
	{
		base.OnCellPressed(cell, mouseButton);

		_viewModel.ShowCellInfoPanelCommand.Execute(cell);
	}

	protected override void OnCellReleased(Vector2I cell)
	{
		base.OnCellReleased(cell);

		_viewModel.HideCellInfoPanelCommand.Execute(null);
	}
}