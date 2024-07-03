using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public abstract class SimulationStatsPanelVM : ObservableObject, IPanelViewModel, IUnsubscribe
{
    public bool IsShown
    {
        get => _panelStatesModel.SimulationStatsPanelShown;
        set => _panelStatesModel.SimulationStatsPanelShown = value;
    }

    private readonly PanelStatesModel _panelStatesModel;

    protected SimulationStatsPanelVM(PanelStatesModel panelStatesModel)
    {
        _panelStatesModel = panelStatesModel;
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.SimulationStatsPanelShown, () => OnPropertyChanged(nameof(IsShown)));
    }

    public virtual void Unsubscribe() =>
        TriggerBinder.UnbindPropertyChanged(this, _panelStatesModel, o => o.SimulationStatsPanelShown);
}