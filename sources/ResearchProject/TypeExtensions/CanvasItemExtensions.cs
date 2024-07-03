using Godot;

public static class CanvasItemExtensions
{
    public static void ToggleSelfModulate(this CanvasItem canvasItem, bool state, Color whenTrue, Color whenFalse) =>
        canvasItem.SelfModulate = state ? whenTrue : whenFalse;
}