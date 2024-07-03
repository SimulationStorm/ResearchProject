public class ArtLifeVM : SimulationVM
{
    #region Properties
    public ArtLifeFieldVM FieldVM { get; init; }

    public ArtLifeFieldUiVM FieldUiVM { get; init; }

    public ArtLifeMenuVM MenuVM { get; init; }

    public ArtLifeStatsPanelVM StatsPanelVM { get; init; }

    public WorldEnvPanelVM WorldEnvPanelVM { get; init; }

    public CellInfoPanelVM CellInfoPanelVM { get; init; }
    #endregion

    public ArtLifeVM
    (
        SimulationManagerModel simulationManagerModel,
        FieldStateModel fieldStateModel,
        PanelStatesModel panelStatesModel,
        ArtLifeModel artLifeModel)
    {
        var presentationModel = new ArtLifePresentationModel();

        FieldVM = new(simulationManagerModel, fieldStateModel, artLifeModel, presentationModel);
        FieldUiVM = new(fieldStateModel, artLifeModel, presentationModel);
        MenuVM = new(panelStatesModel, artLifeModel, presentationModel);
        StatsPanelVM = new(panelStatesModel, artLifeModel);
        WorldEnvPanelVM = new(artLifeModel);
        CellInfoPanelVM = new(presentationModel, artLifeModel);
    }

    public override void Unsubscribe()
    {
        FieldVM.Unsubscribe();
        FieldUiVM.Unsubscribe();
        MenuVM.Unsubscribe();
        StatsPanelVM.Unsubscribe();
        WorldEnvPanelVM.Unsubscribe();
    }
}