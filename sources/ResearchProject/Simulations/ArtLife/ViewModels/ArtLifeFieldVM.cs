using EasyBindings;
using System.Collections.ObjectModel;

public partial class ArtLifeFieldVM : SimulationFieldVM
{
    #region Properties
    public IGetCell<Cell> Field => _artLifeModel.FieldCellGetter;

    public ObservableCollection<Substance> DisplayedSubstances => _presentationModel.DisplayedSubstances;

    public DisplayMode DisplayMode => _presentationModel.DisplayMode;
    #endregion

    #region Fields
    private readonly ArtLifeModel _artLifeModel;

    private readonly ArtLifePresentationModel _presentationModel;
    #endregion

    public ArtLifeFieldVM
    (
        SimulationManagerModel simulationManagerModel,
        FieldStateModel fieldStateModel,
        ArtLifeModel artLifeModel,
        ArtLifePresentationModel presentationModel
    )
    : base(fieldStateModel, simulationManagerModel, artLifeModel)
    {
        _artLifeModel = artLifeModel;

        _presentationModel = presentationModel;
        TriggerBinder.OnPropertyChanged(this, _presentationModel, o => o.DisplayMode, NotifyStateChanged);
        TriggerBinder.OnCollectionChanged(this, _presentationModel.DisplayedSubstances, NotifyStateChanged);
    }
}