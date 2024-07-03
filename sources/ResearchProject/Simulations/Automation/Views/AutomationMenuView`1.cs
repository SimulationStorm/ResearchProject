using Godot;

public abstract partial class AutomationMenuView<TCellState> : SimulationMenuView
{
    protected void Setup(AutomationMenuVM<TCellState> viewModel)
    {
        base.Setup(viewModel);

        SetupDrawingModeView();
        SetupFieldWrappingView();
    }

    [Export] public NodePath DrawingModeViewPath { get; set; } = null!;
    protected abstract void SetupDrawingModeView();

    [Export] public NodePath FieldWrappingViewPath { get; set; } = null!;
    protected abstract void SetupFieldWrappingView();
}