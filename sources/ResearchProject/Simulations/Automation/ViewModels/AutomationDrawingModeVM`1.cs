using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyBindings;
using EasyBindings.Interfaces;

public abstract partial class AutomationDrawingModeVM<TCellState> : ObservableObject, IUnsubscribe
{
    #region Properties
    public bool DrawingModeEnabled
    {
        get => _presentationModel.DrawingModeEnabled;
        set => _presentationModel.DrawingModeEnabled = value;
    }

    public int DrawingBrushRadius
    {
        get => _presentationModel.DrawingBrushRadius;
        set => _presentationModel.DrawingBrushRadius = value;
    }
    #endregion

    #region Commands
    [RelayCommand(CanExecute = nameof(CanSetDrawingBrushShape))]
    private void SetDrawingBrushShape(DrawingBrushShape drawingBrushShape)
    {
        _presentationModel.DrawingBrushShape = drawingBrushShape;
        SetDrawingBrushShapeCommand.NotifyCanExecuteChanged();
    }
    private bool CanSetDrawingBrushShape(DrawingBrushShape drawingBrushShape) => drawingBrushShape != _presentationModel.DrawingBrushShape;


    [RelayCommand(CanExecute = nameof(CanSetDrawingBrushCellState))]
    private void SetDrawingBrushCellState(TCellState cellState)
    {
        _presentationModel.DrawingBrushCellState = cellState;
        SetDrawingBrushCellStateCommand.NotifyCanExecuteChanged();
    }
    private bool CanSetDrawingBrushCellState(TCellState cellState) => !cellState.Equals(_presentationModel.DrawingBrushCellState);
    #endregion

    private readonly AutomationPresentationModel<TCellState> _presentationModel;

    protected AutomationDrawingModeVM(AutomationPresentationModel<TCellState> presentationModel)
    {
        _presentationModel = presentationModel;
        TriggerBinder.OnPropertyChanged(this, _presentationModel, o => o.DrawingModeEnabled, () => OnPropertyChanged(nameof(DrawingModeEnabled)));
        TriggerBinder.OnPropertyChanged(this, _presentationModel, o => o.DrawingBrushRadius, () => OnPropertyChanged(nameof(DrawingBrushRadius)));
    }

    public virtual void Unsubscribe() => TriggerBinder.Unbind(this);
}