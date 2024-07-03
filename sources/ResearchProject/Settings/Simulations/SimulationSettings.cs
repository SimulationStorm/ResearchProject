using System;
using System.Collections.Generic;
using Godot;

public static class SimulationSettings
{
    public const SimulationMode InitialSimulationMode = SimulationMode.UniversalAutomation;

    public const int
        MinIterationsPerSecond = 1,
        MaxIterationsPerSecond = 999, // If user choose greater in simulation => it will become unlimited
        IterationsPerSecondStep = 1,
        InitialIterationsPerSecond = 10,
        
        MinFramesToSkip = 1,
        MaxFramesToSkip = 999, // If user choose greater in simulation => skips all frames
        FramesToSkipStep = 1,
        InitialFramesToSkip = 1;

    public const bool
        InitialSkipFrames = false;

    public static readonly IReadOnlyDictionary<SimulationMode, Type> SimulationModelTypesByMode = new Dictionary<SimulationMode, Type>
    {
        [SimulationMode.LifeLikeAutomation]  = typeof(LifeLikeAutomationModel),
        [SimulationMode.UniversalAutomation] = typeof(UniversalAutomationModel),
        [SimulationMode.ArtLife]             = typeof(ArtLifeModel)
    };

    public static readonly IReadOnlyDictionary<SimulationMode, Type> SimulationViewModelTypesByMode = new Dictionary<SimulationMode, Type>
    {
        [SimulationMode.LifeLikeAutomation]  = typeof(LifeLikeAutomationVM),
        [SimulationMode.UniversalAutomation] = typeof(UniversalAutomationVM),
        [SimulationMode.ArtLife]             = typeof(ArtLifeVM)
    };

    public static IReadOnlyDictionary<SimulationMode, PackedScene> SimulationViewScenesByMode { get; private set; } = null!;

    private const string SimulationViewScenePathPattern = "res://Simulations/{0}/Views/{0}View.tscn";

    public static void Setup() => LoadSimulationViewScenes();

    private static void LoadSimulationViewScenes()
    {
        var simulationViewScenesByMode = new Dictionary<SimulationMode, PackedScene>();
        foreach (var mode in SimulationModeExtensions.Modes)
            simulationViewScenesByMode[mode] = LoadSimulationViewScene(mode);

        SimulationViewScenesByMode = simulationViewScenesByMode;
    }

    private static PackedScene LoadSimulationViewScene(SimulationMode mode) =>
        ResourceLoader.Load<PackedScene>(GetSimulationViewScenePath(mode));

    private static string GetSimulationViewScenePath(SimulationMode mode)
    {
        var simulationModeName = mode.ToString();
        return string.Format(SimulationViewScenePathPattern, simulationModeName);
    }
}

//public interface ISettings
//{
//    void Setup();
//}

//public class SimulationSettings2 : ISettings
//{
//    public const SimulationMode InitialSimulationMode = SimulationMode.UniversalAutomation;

//    public const int
//        MinIterationsPerSecond = 1,
//        MaxIterationsPerSecond = 999, // If user choose greater in simulation => it will become unlimited
//        IterationsPerSecondStep = 1,
//        InitialIterationsPerSecond = 10,

//        MinFramesToSkip = 1,
//        MaxFramesToSkip = 999, // If user choose greater in simulation => skips all frames
//        FramesToSkipStep = 1,
//        InitialFramesToSkip = 1;

//    public const bool
//        InitialSkipFrames = false;

//    public IReadOnlyDictionary<SimulationMode, Type> SimulationModelTypesByMode { get; } = new Dictionary<SimulationMode, Type>
//    {
//        [SimulationMode.LifeLikeAutomation] = typeof(LifeLikeAutomationModel),
//        [SimulationMode.UniversalAutomation] = typeof(UniversalAutomationModel),
//        [SimulationMode.ArtLife] = typeof(ArtLifeModel)
//    };

//    public IReadOnlyDictionary<SimulationMode, Type> SimulationViewModelTypesByMode { get; } = new Dictionary<SimulationMode, Type>
//    {
//        [SimulationMode.LifeLikeAutomation] = typeof(LifeLikeAutomationVM),
//        [SimulationMode.UniversalAutomation] = typeof(UniversalAutomationVM),
//        [SimulationMode.ArtLife] = typeof(ArtLifeVM)
//    };

//    public IReadOnlyDictionary<SimulationMode, PackedScene> SimulationViewScenesByMode { get; private set; }

//    private const string SimulationsDirectory = "res://Simulations/";
//    private readonly IReadOnlyDictionary<SimulationMode, string> _simulationViewScenePathsByMode = new Dictionary<SimulationMode, string>
//    {
//        [SimulationMode.LifeLikeAutomation] = $"{SimulationsDirectory}LifeLikeAutomation/Views/LifeLikeAutomationView.tscn",
//        [SimulationMode.UniversalAutomation] = $"{SimulationsDirectory}UniversalAutomation/Views/UniversalAutomationView.tscn",
//        [SimulationMode.ArtLife] = $"{SimulationsDirectory}ArtLife/Views/ArtLifeView.tscn"
//    };

//    public void Setup() => LoadViewScenes();

//    private void LoadViewScenes()
//    {
//        var simulationViewScenesByMode = new Dictionary<SimulationMode, PackedScene>();
//        foreach (var (mode, viewScenePath) in _simulationViewScenePathsByMode)
//            simulationViewScenesByMode[mode] = ResourceLoader.Load<PackedScene>(viewScenePath);

//        SimulationViewScenesByMode = simulationViewScenesByMode;
//    }
//}

//public class SimulationSettingsV2
//{
//    public static SimulationSettingsV2 Instance { get; private set; } = null!;

//    public const SimulationMode InitialSimulationMode = SimulationMode.UniversalAutomation;

//    public const int
//        MinIterationsPerSecond = 1,
//        MaxIterationsPerSecond = 999, // If user choose greater in simulation => it will become unlimited
//        IterationsPerSecondStep = 1,
//        InitialIterationsPerSecond = 10,

//        MinFramesToSkip = 1,
//        MaxFramesToSkip = 999, // If user choose greater in simulation => skips all frames
//        FramesToSkipStep = 1,
//        InitialFramesToSkip = 1;

//    public const bool
//        InitialSkipFrames = false;

//    public IReadOnlyDictionary<SimulationMode, PackedScene> SimulationViewScenesByMode { get; }

//    private readonly IReadOnlyDictionary<SimulationMode, string> _simulationViewScenePathsByMode = new Dictionary<SimulationMode, string>
//    {
//        [SimulationMode.LifeLikeAutomation] = "res://Simulations/LifeLikeAutomation/Views/LifeLikeAutomationView.tscn",
//        [SimulationMode.UniversalAutomation] = "res://Simulations/UniversalAutomation/Views/UniversalAutomationView.tscn",
//        [SimulationMode.ArtLife] = "res://Simulations/ArtLife/Views/ArtLifeView.tscn"
//    };

//    public SimulationSettingsV2()
//    {
//        Instance = this;

//        var simulationViewScenesByMode = new Dictionary<SimulationMode, PackedScene>();
//        foreach (var (mode, viewScenePath) in _simulationViewScenePathsByMode)
//            simulationViewScenesByMode[mode] = ResourceLoader.Load<PackedScene>(viewScenePath);
//        SimulationViewScenesByMode = simulationViewScenesByMode;
//    }
//}

//public class SimulationSettingsV3 : ISettings
//{
//    public const SimulationMode InitialSimulationMode = SimulationMode.UniversalAutomation;

//    public const int
//        MinIterationsPerSecond = 1,
//        MaxIterationsPerSecond = 999, // If user choose greater in simulation => it will become unlimited
//        IterationsPerSecondStep = 1,
//        InitialIterationsPerSecond = 10,

//        MinFramesToSkip = 1,
//        MaxFramesToSkip = 999, // If user choose greater in simulation => skips all frames
//        FramesToSkipStep = 1,
//        InitialFramesToSkip = 1;

//    public const bool
//        InitialSkipFrames = false;

//    public IReadOnlyDictionary<SimulationMode, PackedScene> SimulationViewScenesByMode { get; private set; }

//    private readonly IReadOnlyDictionary<SimulationMode, string> _simulationViewScenePathsByMode = new Dictionary<SimulationMode, string>
//    {
//        [SimulationMode.LifeLikeAutomation] = "res://Simulations/LifeLikeAutomation/Views/LifeLikeAutomationView.tscn",
//        [SimulationMode.UniversalAutomation] = "res://Simulations/UniversalAutomation/Views/UniversalAutomationView.tscn",
//        [SimulationMode.ArtLife] = "res://Simulations/ArtLife/Views/ArtLifeView.tscn"
//    };

//    public void Setup()
//    {
//        LoadViewScenes();
//    }

//    private void LoadViewScenes()
//    {
//        var simulationViewScenesByMode = new Dictionary<SimulationMode, PackedScene>();
//        foreach (var (mode, viewScenePath) in _simulationViewScenePathsByMode)
//            simulationViewScenesByMode[mode] = ResourceLoader.Load<PackedScene>(viewScenePath);

//        SimulationViewScenesByMode = simulationViewScenesByMode;
//    }
//}