using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public abstract class SimulationFieldVM : ObservableObject, INotifyStateChanged, IUnsubscribe
{
    #region Properties
    public object? State { get; }

    public float CellSize => (float)_fieldStateModel.CellSize;

    public Vector2I FieldSize => _fieldStateModel.FieldSize;

    public bool GridLinesShown => _fieldStateModel.GridLinesShown;

    public Color GridLinesColor => _fieldStateModel.GridLinesColor;
    #endregion

    #region Fields
    private readonly FieldStateModel _fieldStateModel;

    private readonly SimulationManagerModel _simulationManagerModel;

    private readonly SimulationModel _simulationModel;
    #endregion

    protected SimulationFieldVM(FieldStateModel fieldStateModel, SimulationManagerModel simulationManagerModel, SimulationModel simulationModel)
    {
        _fieldStateModel = fieldStateModel;
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.CellSize, NotifyStateChanged);
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.GridLinesShown, NotifyStateChanged);
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.GridLinesColor, NotifyStateChanged);

        _simulationManagerModel = simulationManagerModel;
        TriggerBinder.OnPropertyChanged(this, simulationManagerModel, o => o.FieldRedrawRequired, fieldRedrawRequired =>
        {
            if (fieldRedrawRequired)
                NotifyStateChanged();
        });

        _simulationModel = simulationModel;
        TriggerBinder.OnPropertyChanged(this, _simulationModel, o => o.State, NotifyStateChanged);
        TriggerBinder.OnPropertyChanged(this, _simulationModel, o => o.Advanced, NotifyStateChanged);
        TriggerBinder.OnPropertyChanged(this, _simulationModel, o => o.WasReset, NotifyStateChanged);
    }

    protected void NotifyStateChanged() => OnPropertyChanged(nameof(State));

    public virtual void Unsubscribe()
    {
        TriggerBinder.UnbindPropertyChanged(this, _fieldStateModel, o => o.CellSize);
        TriggerBinder.UnbindPropertyChanged(this, _fieldStateModel, o => o.GridLinesShown);
        TriggerBinder.UnbindPropertyChanged(this, _fieldStateModel, o => o.GridLinesColor);

        TriggerBinder.UnbindPropertyChanged(this, _simulationManagerModel, o => o.FieldRedrawRequired);

        TriggerBinder.UnbindPropertyChanged(this, _simulationModel, o => o.State);
        TriggerBinder.UnbindPropertyChanged(this, _simulationModel, o => o.Advanced);
        TriggerBinder.UnbindPropertyChanged(this, _simulationModel, o => o.WasReset);
    }
}