using EasyBindings;

public partial class LifeLikeAutomationDrawingModeView : AutomationDrawingModeView<LifeLikeAutomationCellState>, IView<LifeLikeAutomationDrawingModeVM>
{
    private LifeLikeAutomationDrawingModeVM _viewModel = null!;

    public void Setup(LifeLikeAutomationDrawingModeVM viewModel)
    {
        _viewModel = viewModel;
        base.Setup(viewModel);
    }

    protected override void SetupBrushCellStateButtons()
    {
        foreach (var state in LifeLikeAutomationCellStateExtensions.All)
        {
            var button = new RichButton
            {
                Text = state.Name(),
                FocusMode = FocusModeEnum.None,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };

            CommandBinder.Bind(this, button, _viewModel.SetDrawingBrushCellStateCommand, () =>
                LifeLikeAutomationCellStateExtensions.ByName(button.Text));

            BrushCellStatesBox.AddChild(button);
        }
    }
}