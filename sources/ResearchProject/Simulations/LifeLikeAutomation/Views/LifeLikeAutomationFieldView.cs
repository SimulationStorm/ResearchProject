using Godot;

public partial class LifeLikeAutomationFieldView : SimulationFieldView, IView<LifeLikeAutomationFieldVM>
{
	private LifeLikeAutomationFieldVM _viewModel = null!;

    public void Setup(LifeLikeAutomationFieldVM viewModel)
	{
		_viewModel = viewModel;
		base.Setup(viewModel);
    }

    #region Protected methods
    protected override Color GetBackgroundColor() => _viewModel.CellColorsByState[LifeLikeAutomationCellState.Dead];

    protected override void DrawCells()
    {
        if (_viewModel.Algorithm is LifeLikeAutomationBitwiseAlgorithm)
            DrawCellsOfBitwiseAlgorithm();
        else
            DrawCellsOfSmartAlgorithm();
    }
    #endregion

    #region Private methods
    private void DrawCellsOfBitwiseAlgorithm()
    {
        var bitwiseAlgorithm = LifeLikeAutomationBitwiseAlgorithm.Instance;

        int columns = _viewModel.FieldSize.X,
            rows = _viewModel.FieldSize.Y;
        var cellSize = _viewModel.CellSize;

        var aliveCellColor = _viewModel.CellColorsByState[LifeLikeAutomationCellState.Alive];

        for (var y = 0; y < rows; y++)
            for (var x = 0; x < columns; x++)
                if (bitwiseAlgorithm.GetCellState(new(x, y)) is LifeLikeAutomationCellState.Alive)
                    DrawRect(new Rect2(x * cellSize, y * cellSize, cellSize, cellSize), aliveCellColor);
    }

    private void DrawCellsOfSmartAlgorithm()
    {
        var aliveCells = LifeLikeAutomationSmartAlgorithm.Instance.AliveCells;

        var cellSize = _viewModel.CellSize;
        var aliveCellColor = _viewModel.CellColorsByState[LifeLikeAutomationCellState.Alive];

        foreach (var (x, y) in aliveCells)
            DrawRect(new Rect2(x * cellSize, y * cellSize, cellSize, cellSize), aliveCellColor);
    }
    #endregion
}