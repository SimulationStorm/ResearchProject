using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public class SettingsPanelVM : ObservableObject, IPanelViewModel, IUnsubscribe
{
    #region Properties

    public bool IsShown
    {
        get => _panelStatesModel.SettingsPanelShown;
        set => _panelStatesModel.SettingsPanelShown = value;
    }

    public string ApplicationLocale
    {
        get => App.Locale;
        set => App.Locale = value;
    }

    public Color FieldBackgroundColor
    {
        get => App.DefaultClearColor;
        set => App.DefaultClearColor = value;
    }

    public bool VSyncEnabled
    {
        get => App.VSyncEnabled;
        set => App.VSyncEnabled = value;
    }
    #endregion


    //[RelayCommand]
    //private void ResetSettings()
    //{
    //    // TODO: Here, we can reset all settings to their default/initial values
    //}

    private readonly PanelStatesModel _panelStatesModel;

    public SettingsPanelVM(PanelStatesModel panelStatesModel)
    {
        _panelStatesModel = panelStatesModel;
        TriggerBinder.OnPropertyChanged(this, panelStatesModel, o => o.SettingsPanelShown, () => OnPropertyChanged(nameof(IsShown)));
    }

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}