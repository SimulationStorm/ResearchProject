using Godot;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using EasyBindings;
using EasyBindings.Interfaces;

public class ControlPanelVM : ObservableObject, IPanelViewModel, IUnsubscribe
{
    #region Properties
    public int IterationsPerSecond
    {
        get => _simulationManagerModel.IterationsPerSecond;
        set => _simulationManagerModel.IterationsPerSecond = value;
    }

    public int FramesToSkip
    {
        get => _simulationManagerModel.FramesToSkip;
        set => _simulationManagerModel.FramesToSkip = value;
    }

    public bool SkipFrames
    {
        get => _simulationManagerModel.SkipFrames;
        set => _simulationManagerModel.SkipFrames = value;
    }

    public double ViewScale
    {
        get => _fieldStateModel.ViewScale;
        set => _fieldStateModel.ViewScale = value;
    }

    public double CellSize
    {
        get => _fieldStateModel.CellSize;
        set => _fieldStateModel.CellSize = value;
    }

    public IReadOnlyList<double> AvailableCellSizes => _fieldStateModel.AvailableCellSizes;

    public bool GridLinesShown
    {
        get => _fieldStateModel.GridLinesShown;
        set => _fieldStateModel.GridLinesShown = value;
    }

    public Color GridLinesColor
    {
        get => _fieldStateModel.GridLinesColor;
        set => _fieldStateModel.GridLinesColor = value;
    }

    public bool IsShown
    {
        get => _panelStatesModel.ControlPanelShown;
        set => _panelStatesModel.ControlPanelShown = value;
    }
    #endregion

    #region Fields
    private readonly SimulationManagerModel _simulationManagerModel;
    
    private readonly FieldStateModel _fieldStateModel;
    
    private readonly PanelStatesModel _panelStatesModel;
    #endregion

    public ControlPanelVM(SimulationManagerModel simulationManagerModel, FieldStateModel fieldStateModel, PanelStatesModel panelStatesModel)
    {
        _simulationManagerModel = simulationManagerModel;
        TriggerBinder.OnPropertyChanged(this, _simulationManagerModel, o => o.IterationsPerSecond, () => OnPropertyChanged(nameof(IterationsPerSecond)));
        TriggerBinder.OnPropertyChanged(this, _simulationManagerModel, o => o.SkipFrames, () => OnPropertyChanged(nameof(SkipFrames)));
        TriggerBinder.OnPropertyChanged(this, _simulationManagerModel, o => o.FramesToSkip, () => OnPropertyChanged(nameof(FramesToSkip)));

        _fieldStateModel = fieldStateModel;
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.ViewScale, () => OnPropertyChanged(nameof(ViewScale)));
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.CellSize, () => OnPropertyChanged(nameof(CellSize)));
        TriggerBinder.OnPropertyChanged(this, _fieldStateModel, o => o.GridLinesShown, () => OnPropertyChanged(nameof(GridLinesShown)));

        _panelStatesModel = panelStatesModel;
        TriggerBinder.OnPropertyChanged(this, _panelStatesModel, o => o.ControlPanelShown, () => OnPropertyChanged(nameof(IsShown)));
    }
    public void Unsubscribe() => TriggerBinder.Unbind(this);
}