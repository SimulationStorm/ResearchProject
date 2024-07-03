using CommunityToolkit.Mvvm.ComponentModel;

public abstract partial class AutomationPresentationModel<TCellState> : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private bool _drawingModeEnabled;

    [ObservableProperty]
    private DrawingBrushShape _drawingBrushShape;

    [ObservableProperty]
    private int _drawingBrushRadius;

    [ObservableProperty]
    private TCellState _drawingBrushCellState;
    #endregion

    protected AutomationPresentationModel(TCellState initialDrawingBrushCellState)
    {
        DrawingModeEnabled = AutomationSettings.InitialDrawingModeEnabled;
        DrawingBrushShape = AutomationSettings.InitialDrawingBrushShape;
        DrawingBrushRadius = AutomationSettings.InitialDrawingBrushRadius;
        DrawingBrushCellState = initialDrawingBrushCellState;
    }
}