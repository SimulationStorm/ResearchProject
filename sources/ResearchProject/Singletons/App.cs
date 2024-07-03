using Godot;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;

public partial class App : Node
{
    #region Properties
    public static int Fps => (int)Engine.GetFramesPerSecond();

    public static Vector2I ScreenSize => DisplayServer.ScreenGetSize();

    public static bool VSyncEnabled
    {
        get => DisplayServer.WindowGetVsyncMode() == DisplayServer.VSyncMode.Enabled;
        set => DisplayServer.WindowSetVsyncMode(value ? DisplayServer.VSyncMode.Enabled : DisplayServer.VSyncMode.Disabled);
    }

    public static Color DefaultClearColor
    {
        get => RenderingServer.GetDefaultClearColor();
        set => RenderingServer.SetDefaultClearColor(value);
    }

    public static string Locale
    {
        get => TranslationServer.GetLocale();
        set => TranslationServer.SetLocale(value);
    }
    #endregion

    public static event Action? Process
    {
        add => _instance.PrivateProcess += value;
        remove => _instance.PrivateProcess -= value;
    }

    private static App _instance = null!;

    private event Action? PrivateProcess;

    #region Setting up
    public App()
    {
        _instance = this;

        AppSettings.Setup();
        SimulationSettings.Setup();
        UniversalAutomationSettings.Setup();

        SetupLiveCharts();
    }

    private void SetupLiveCharts()
    {
        LiveCharts.Configure(config =>
        {
            // TODO: Set Droid Sans font
            config.HasGlobalSKTypeface(SKFontManager.Default.MatchFamily("Comic Sans MS"));
            config.LegendTextPaint = new SolidColorPaint { Color = SKColors.White };
        });
    }
    #endregion
    
    public static void Quit() => _instance.GetTree().Quit();

    public override void _Process(double delta)
    {
        base._Process(delta);

        PrivateProcess?.Invoke();
    }
}