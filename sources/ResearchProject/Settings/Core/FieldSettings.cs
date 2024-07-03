using Godot;

public static class FieldSettings
{
    public const double
        MinCellSize = 1,
        CellSizeStep = 0.25;

    public const bool InitialGridLinesShown = true;
    public static readonly Color InitialGridLinesColor = Colors.Black;

    public static readonly Color HoveredCellColor = Colors.Green with { A = 0.33f },
        PressedCellColor = Colors.Green with { A = 0.66f };
}