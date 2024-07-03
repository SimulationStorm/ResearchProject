using System.Collections.Generic;
using Godot;

public static class AppSettings
{
    #region Locale
    /// <summary>
    /// Locale codes and their names
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> LocalesByName = new Dictionary<string, string>
    {
        { "en", "English" },
        { "ru", "Русский" }
    };

    /// <summary>
    /// Locale that will be used if user's system locale is not supported
    /// </summary>
    private const string DefaultLocale = "en";

    /// <summary>
    /// Initial locale will be user's system locale, or <see cref="DefaultLocale"/> if it is not supported
    /// </summary>
    public static string InitialLocale { get; private set; } = null!;

    /// <summary>
    /// Action names
    /// </summary>
    public const string
        QuitAppActionName = "quit_app",
        
        ToggleBasicInfoPanelActionName = "toggle_basic_info_panel",
        ToggleControlPanelActionName = "toggle_control_panel",
        ToggleHelpPanelActionName = "toggle_help_panel",
        ToggleSettingsPanelActionName = "toggle_settings_panel",
        ToggleSimulationMenuActionName = "toggle_simulation_menu",
        ToggleSimulationStatsPanelActionName = "toggle_simulation_stats_panel";
    #endregion

    #region Setting up
    public static void Setup() => SetupInitialLocale();

    private static void SetupInitialLocale()
    {
        var systemLocale = OS.GetLocaleLanguage();
        InitialLocale = LocalesByName.ContainsKey(systemLocale) ? systemLocale : DefaultLocale;
    }
    #endregion
}