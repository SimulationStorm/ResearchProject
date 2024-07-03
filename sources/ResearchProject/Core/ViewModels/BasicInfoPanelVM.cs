using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public class BasicInfoPanelVM : ObservableObject, IPanelViewModel, IUnsubscribe
{
    #region Properties
    public bool IsShown
    {
        get => _panelStatesModel.BasicInfoPanelShown;
        set => _panelStatesModel.BasicInfoPanelShown = value;
    }

    public int Fps => App.Fps;

    public Vector2I FieldSize => _fieldStateModel.FieldSize;

    public int FieldCellCount => FieldSize.X * FieldSize.Y;

    public int IterationNumber => _simulationManagerModel.IterationNumber;
    #endregion

    #region Fields
    private readonly SimulationManagerModel _simulationManagerModel;

    private readonly FieldStateModel _fieldStateModel;

    private readonly PanelStatesModel _panelStatesModel;
    #endregion

    public BasicInfoPanelVM(SimulationManagerModel simulationManagerModel, FieldStateModel fieldStateModel, PanelStatesModel panelStatesModel)
    {
        _simulationManagerModel = simulationManagerModel;
        TriggerBinder.OnPropertyChanged(this, _simulationManagerModel, o => o.IterationNumber, () => OnPropertyChanged(nameof(IterationNumber)));

        _fieldStateModel = fieldStateModel;
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.FieldSize, () =>
        {
            OnPropertyChanged(nameof(FieldSize));
            OnPropertyChanged(nameof(FieldCellCount));
        });

        _panelStatesModel = panelStatesModel;
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.BasicInfoPanelShown, () => OnPropertyChanged(nameof(IsShown)));
    }

    public void Unsubscribe() => TriggerBinder.Unbind(this);
}