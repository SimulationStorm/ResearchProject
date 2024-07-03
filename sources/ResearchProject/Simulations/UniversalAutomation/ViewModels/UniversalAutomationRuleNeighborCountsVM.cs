using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public partial class UniversalAutomationRuleNeighborCountsVM : ObservableObject, INotifyStateChanged, IUnsubscribe
{
    #region Properties
    public object? State { get; }
    
    public int NeighborhoodRadius => _neighborhoodVm.Radius;

    public object? NeighborhoodState { get; }

    public int MaxNeighborCount => _neighborhoodVm.SelectedPositions.Count;
    #endregion

    private readonly UniversalAutomationNeighborhoodVM _neighborhoodVm;

    public UniversalAutomationRuleNeighborCountsVM(UniversalAutomationNeighborhoodVM neighborhoodVm)
    {
        _neighborhoodVm = neighborhoodVm;
        TriggerBinder.OnPropertyChanged(this, _neighborhoodVm, o => o.Radius, () => OnPropertyChanged(nameof(NeighborhoodRadius)));
        TriggerBinder.OnPropertyChanged(this, _neighborhoodVm, o => o.State, () => OnPropertyChanged(nameof(NeighborhoodState)));
    }

    #region Methods
    //public bool IsNeighborCountAdded(int neighborCount)
    //{

    //}

    public void AddNeighborCount(int neighborCount)
    {
        OnPropertyChanged(nameof(State));
    }

    public void RemoveNeighborCount(int neighborCount)
    {
        OnPropertyChanged(nameof(State));
    }
    #endregion

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}