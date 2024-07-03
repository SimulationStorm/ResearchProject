using Godot;

public static class UniversalAutomationSettings
{
    public const int
        MinNeighborhoodRadius = 1,
        MaxNeighborhoodRadius = 5,

        InitialRandomSeed = 0,
        MaxRuleSetsRepetitionsNumber = 100,
        MaxRuleSetApplicationsNumber = 100,
        
        MaxStatesNumber = MaxState - MinState + 1;

    public const byte
        EmptyState = 0,
        MinState = 1,
        MaxState = 255;

    public const bool
        InitialDrawModeEnabled = false;

    public const AutomationFieldWrapping
        InitialFieldWrapping = AutomationFieldWrapping.NoWrap;

    public const double MinRuleProbability = 0,
                        MaxRuleProbability = 1,
                        RuleProbabilityStep = 0.01;

    public static readonly UniversalAutomationKind
        InitialKind = UniversalAutomationKind.WireWorld;

    public static PackedScene StateView { get; private set; } = null!;
    public static PackedScene RuleSetView { get; private set; } = null!;
    public static PackedScene RuleView { get; private set; } = null!;

    private const string ViewScenePathPattern = "res://Simulations/UniversalAutomation/Views/UniversalAutomation{0}.tscn";

    #region Setting up
    public static void Setup() => LoadViewScenes();

    private static void LoadViewScenes()
    {
        StateView = LoadViewScene(nameof(StateView));
        RuleSetView = LoadViewScene(nameof(RuleSetView));
        RuleView = LoadViewScene(nameof(RuleView));
    }

    private static PackedScene LoadViewScene(string viewSceneName) => ResourceLoader.Load<PackedScene>(GetViewScenePath(viewSceneName));

    private static string GetViewScenePath(string viewSceneName) => string.Format(ViewScenePathPattern, viewSceneName);
    #endregion
}