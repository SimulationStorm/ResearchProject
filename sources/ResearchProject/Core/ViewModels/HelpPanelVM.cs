using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public class HelpPanelVM : ObservableObject, IPanelViewModel, IUnsubscribe
{
    public bool IsShown
    {
        get => _panelStatesModel.HelpPanelShown;
        set => _panelStatesModel.HelpPanelShown = value;
    }

    private readonly PanelStatesModel _panelStatesModel;

    public HelpPanelVM(PanelStatesModel panelStatesModel)
    {
        _panelStatesModel = panelStatesModel;
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.HelpPanelShown, () => OnPropertyChanged(nameof(IsShown)));
    }

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}