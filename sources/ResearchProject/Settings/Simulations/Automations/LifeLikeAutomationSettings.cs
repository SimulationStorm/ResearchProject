using Godot;

public static class LifeLikeAutomationSettings
{
	// Only one of kind/rule should be set as initial
	public static readonly LifeLikeAutomationKind?
        InitialAutomationKind = LifeLikeAutomationKind.GameOfLife;

	public static readonly LifeLikeAutomationRule?
        InitialAutomationRule = null;

	public static readonly LifeLikeAutomationAlgorithm?
		InitialAlgorithm = LifeLikeAutomationAlgorithm.Bitwise;

    public const AutomationFieldWrapping
        InitialFieldWrapping = AutomationFieldWrapping.NoWrap;

    public const double
		MinLiveDensity = 0,
		MaxLiveDensity = 1,
		LiveDensityStep = 0.01,
		InitialLiveDensity = 0.01;

	public static readonly Color
		InitialDeadCellColor = Colors.White,
        InitialAliveCellColor = Colors.Black;

	public const LifeLikeAutomationCellState
		InitialBrushCellState = LifeLikeAutomationCellState.Dead;
}