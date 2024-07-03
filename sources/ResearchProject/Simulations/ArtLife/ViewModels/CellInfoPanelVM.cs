using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public partial class CellInfoPanelVM : ObservableObject, IPanelViewModel, INotifyStateChanged, IUnsubscribe
{
    #region Properties
    public object? State { get; }

    [ObservableProperty]
    private bool _isShown;

    public Cell? SelectedCell => _artLifePresentationModel.SelectedCell;

    public double Temperature => _artLifePresentationModel.SelectedCell?.Temperature ?? 0;

    public double Acidity => _artLifePresentationModel.SelectedCell?.Acidity ?? 0;

    private static readonly SubstancesContainer _dummySubstancesContainer = new();
    public SubstancesContainer Substances => _artLifePresentationModel.SelectedCell?.Substances ?? _dummySubstancesContainer;
    #endregion

    private readonly ArtLifePresentationModel _artLifePresentationModel;

    public CellInfoPanelVM(ArtLifePresentationModel artLifePresentationModel, ArtLifeModel artLifeModel)
    {
        _artLifePresentationModel = artLifePresentationModel;

        void notifyStateChangedIfCellSelected()
        {
            if (SelectedCell != null)
                OnPropertyChanged(nameof(State));
        }

        TriggerBinder.OnPropertyChanged(this, artLifePresentationModel, o => o.SelectedCell, selectedCell =>
        {
            IsShown = selectedCell != null;
            notifyStateChangedIfCellSelected();
        });

        TriggerBinder.OnPropertyChanged(this, artLifeModel, o => o.State, notifyStateChangedIfCellSelected);
    }

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}