using Godot;

public partial class LifeLikeAutomationFieldUiView : SimulationFieldUiView, IView<LifeLikeAutomationFieldUiVM>
{
    private LifeLikeAutomationFieldUiVM _viewModel = null!;

    public void Setup(LifeLikeAutomationFieldUiVM viewModel)
    {
        _viewModel = viewModel;
        base.Setup(viewModel);
    }

    protected override void OnCellPressed(Vector2I cell, MouseButton mouseButton)
    {
        if (_viewModel.PlacePatternCommand.CanExecute(cell))
        {
            _viewModel.PlacePatternCommand.Execute(cell);
        }
        else
        {
            if (_previousPressedCell is { } previousPressedCell && _viewModel.DrawLineOfShapesCommand.CanExecute((previousPressedCell, cell)))
            {
                _viewModel.DrawLineOfShapesCommand.Execute((previousPressedCell, cell));
            }
            else if (_viewModel.DrawShapeCommand.CanExecute(cell))
            {
                _viewModel.DrawShapeCommand.Execute(cell);
            }
        }

        base.OnCellPressed(cell, mouseButton);
    }
}