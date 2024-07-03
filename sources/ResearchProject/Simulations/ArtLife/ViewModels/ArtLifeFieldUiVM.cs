using CommunityToolkit.Mvvm.Input;
using Godot;

public partial class ArtLifeFieldUiVM : SimulationFieldUiVM
{
    #region Commands
    [RelayCommand]
    private void ShowCellInfoPanel(Vector2I cellCoords) =>
        _presentationModel.SelectedCell = _artLifeModel.FieldCellGetter.GetCell(cellCoords.X, cellCoords.Y);


    [RelayCommand]
    private void HideCellInfoPanel() => _presentationModel.SelectedCell = null;
    #endregion

    #region Fields
    private readonly ArtLifeModel _artLifeModel;

    private readonly ArtLifePresentationModel _presentationModel;
    #endregion

    public ArtLifeFieldUiVM
    (
        FieldStateModel fieldStateModel,
        ArtLifeModel artLifeModel,
        ArtLifePresentationModel presentationModel
    )
    : base(fieldStateModel)
    {
        _artLifeModel = artLifeModel;
        _presentationModel = presentationModel;
    }
}