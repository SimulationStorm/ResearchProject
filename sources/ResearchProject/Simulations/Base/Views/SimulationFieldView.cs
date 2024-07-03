using EasyBindings;
using EasyBindings.Interfaces;
using Godot;

public abstract partial class SimulationFieldView : Node2D, IUnsubscribe
{
    private SimulationFieldVM _viewModel = null!;

    #region Setting up
    public override void _Ready() => Visible = false; /* Preventing premature drawing */

    protected void Setup(SimulationFieldVM viewModel)
    {
        _viewModel = viewModel;

        TriggerBinder.OnPropertyChanged(this, viewModel, o => o.State, QueueRedraw);

        Visible = true;
    }
    #endregion

    #region View
    public override void _Draw()
    {
        DrawBackground();
        DrawCells();
        DrawGridLines();
    }
    #endregion

    #region Protected methods
    protected virtual Color GetBackgroundColor() => default;

    protected virtual Color GetCellColor(int x, int y) => default;

    protected virtual bool CanSkipDrawCell(int x, int y) => false;

    protected virtual void DrawCells()
    {
        int columns = _viewModel.FieldSize.X,
            rows = _viewModel.FieldSize.Y;
        var cellSize = _viewModel.CellSize;

        for (var y = 0; y < rows; y++)
            for (var x = 0; x < columns; x++)
                if (!CanSkipDrawCell(x, y))
                    DrawRect(new Rect2(x * cellSize, y * cellSize, cellSize, cellSize), GetCellColor(x, y));
    }
    #endregion

    #region Private methods
    private void DrawBackground() =>
        DrawRect(new Rect2(0, 0, _viewModel.FieldSize.X * _viewModel.CellSize,
            _viewModel.FieldSize.Y * _viewModel.CellSize), GetBackgroundColor());

    private bool CanSkipDrawGridLines() => !_viewModel.GridLinesShown || _viewModel.CellSize < 2;

    private void DrawGridLines()
    {
        if (CanSkipDrawGridLines())
            return;

        int columns = _viewModel.FieldSize.X,
            rows = _viewModel.FieldSize.Y;
        var cellSize = _viewModel.CellSize;

        var linesColor = _viewModel.GridLinesColor;

        // Vertical lines
        for (var x = 0; x <= columns; x++)
            DrawLine(new Vector2(x * cellSize, 0), new Vector2(x * cellSize, rows * cellSize), linesColor);

        // Horizontal lines
        for (var y = 0; y <= rows; y++)
            DrawLine(new Vector2(0, y * cellSize), new Vector2(columns * cellSize, y * cellSize), linesColor);
    }
    #endregion

    public virtual void Unsubscribe() => TriggerBinder.UnbindPropertyChanged(this, _viewModel, o => o.State);
}