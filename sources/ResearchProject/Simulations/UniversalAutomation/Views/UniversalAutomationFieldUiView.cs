using Godot;

public partial class UniversalAutomationFieldUiView : SimulationFieldUiView, IView<UniversalAutomationFieldUiVM>
{
	private UniversalAutomationFieldUiVM _viewModel = null!;

	public void Setup(UniversalAutomationFieldUiVM viewModel)
	{
		base.Setup(viewModel);
		_viewModel = viewModel;
	}

    protected override void OnCellPressed(Vector2I cell, MouseButton mouseButton)
    {
        if (_previousPressedCell is { } previousPressedCell && _viewModel.DrawLineOfShapesCommand.CanExecute((previousPressedCell, cell)))
        {
            _viewModel.DrawLineOfShapesCommand.Execute((previousPressedCell, cell));
        }
        else if (_viewModel.DrawShapeCommand.CanExecute(cell))
        {
            _viewModel.DrawShapeCommand.Execute(cell);
        }

        base.OnCellPressed(cell, mouseButton);
    }
}