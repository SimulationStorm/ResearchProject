using EasyBindings.Interfaces;
using Godot;

public partial class SimulationFieldUiView : Node2D, IUnsubscribe
{
    #region Fields
    private SimulationFieldUiVM _viewModel = null!;

    private bool _isCellUnderMousePressed;
    
    protected Vector2I? _previousPressedCell;
    #endregion

    #region Setting up

    protected SimulationFieldUiView() => AttachCameraView();
    #endregion

    protected void Setup(SimulationFieldUiVM viewModel)
    {
        _viewModel = viewModel;

        SetupCameraView();
    }

    #region Camera view
    private SimulationFieldCameraView _cameraView = null!;

    private void AttachCameraView()
    {
        _cameraView = new SimulationFieldCameraView();
        AddChild(_cameraView);
    }

    private void SetupCameraView()
    {
        _cameraView.Setup(_viewModel.CameraVM);
        _cameraView.MakeCurrent();
    }
    #endregion

    #region View
    public override void _Draw()
    {
        if (GetCellUnderMouse() is { } cellUnderMouse)
            HighlightCell(cellUnderMouse);
    }
    #endregion

    #region Controller
    public override void _UnhandledInput(InputEvent @event)
    {
        QueueRedraw();

        if (GetCellUnderMouse() is not { } cellUnderMouse)
        {
            OnMouseGoneBeyondField();
            return;
        }

        OnCellHovered(cellUnderMouse);

        // TODO: Camera and field key combinations ?

        if (Input.IsMouseButtonPressed(MouseButton.Left))
            OnCellPressed(cellUnderMouse, MouseButton.Left);
        else if (Input.IsMouseButtonPressed(MouseButton.Right))
            OnCellPressed(cellUnderMouse, MouseButton.Right);
        else
            OnCellReleased(cellUnderMouse);

    }
    #endregion

    #region Protected methods

    protected virtual void OnMouseGoneBeyondField() => _previousPressedCell = null;

    protected virtual void OnCellHovered(Vector2I cell) { }

    protected virtual void OnCellPressed(Vector2I cell, MouseButton mouseButton)
    {
        _previousPressedCell = cell;
        _isCellUnderMousePressed = true;
    }

    protected virtual void OnCellReleased(Vector2I cell)
    {
        _previousPressedCell = null;
        _isCellUnderMousePressed = false;
    }
    #endregion

    #region Private methods
    private void HighlightCell(Vector2I cell)
    {
        var cellSize = _viewModel.CellSize;

        var highlightColor = _isCellUnderMousePressed
            ? FieldSettings.PressedCellColor
            : FieldSettings.HoveredCellColor;

        DrawRect(new Rect2(cell.X * cellSize, cell.Y * cellSize, cellSize, cellSize), highlightColor);
    }

    private Vector2I? GetCellUnderMouse()
    {
        var currGlobalMousePos = GetGlobalMousePosition();

        int x = (int)(currGlobalMousePos.X / _viewModel.CellSize),
            y = (int)(currGlobalMousePos.Y / _viewModel.CellSize);

        if (x < 0 || x >= _viewModel.FieldSize.X || y < 0 || y >= _viewModel.FieldSize.Y)
            return null;

        return new(x, y);
    }
    #endregion

    public virtual void Unsubscribe() => _cameraView.Unsubscribe();
}