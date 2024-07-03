using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public abstract class SimulationMenuVM : ObservableObject, IPanelViewModel, IUnsubscribe
{
    public bool IsShown
    {
        get => _panelStatesModel.SimulationMenuShown;
        set => _panelStatesModel.SimulationMenuShown = value;
    }

    private readonly PanelStatesModel _panelStatesModel;

    protected SimulationMenuVM(PanelStatesModel panelStatesModel)
    {
        _panelStatesModel = panelStatesModel;
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.SimulationMenuShown, () => OnPropertyChanged(nameof(IsShown)));
    }

    public virtual void Unsubscribe() =>
        TriggerBinder.UnbindPropertyChanged(this, _panelStatesModel, o => o.SimulationMenuShown);
}