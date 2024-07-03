using EasyBindings;
using EasyBindings.Interfaces;

public class ArtLifeStatsPanelVM : SimulationStatsPanelVM, INotifyStateChanged
{
    #region Properties
    public object? State { get; }

    public double AverageTemperature => _worldStatistics.AverageTemperature;

    public double AverageAcidity => _worldStatistics.AverageAcidity;

    public SubstancesContainer TotalSubstances => _worldStatistics.TotalSubstanceAmounts;
    #endregion

    private readonly WorldStatistics _worldStatistics;

    public ArtLifeStatsPanelVM
    (
        PanelStatesModel panelStatesModel,
        ArtLifeModel artLifeModel
    )
    : base(panelStatesModel)
    {
        _worldStatistics = artLifeModel.WorldStatistics;
        TriggerBinder.OnPropertyChanged(this, artLifeModel, o => o.State, () => OnPropertyChanged(nameof(State)));
    }
}