public class LifeLikeAutomationHistory : SimulationHistory<LifeLikeAutomationHistoryRecord>
{
    private readonly LifeLikeAutomation _automation;

    public LifeLikeAutomationHistory(LifeLikeAutomation automation) => _automation = automation;

    protected override LifeLikeAutomationHistoryRecord CreateRecord()
    {
        int totalCellCount = _automation.FieldSize.X * _automation.FieldSize.Y,
            aliveCellCount = CountAliveCells(),
            deadCellCount = totalCellCount - aliveCellCount;

        return new()
        {
            DeadCellCount = deadCellCount,
            AliveCellCount = aliveCellCount
        };

        //return new(new Dictionary<LifeLikeAutomationCellState, int>()
        //{
        //    [LifeLikeAutomationCellState.Dead] = deadCellCount,
        //    [LifeLikeAutomationCellState.Alive] = aliveCellCount
        //});
    }

    #region Private methods
    private int CountAliveCells()
    {
        if (_automation.Algorithm is LifeLikeAutomationBitwiseAlgorithm)
            return CountAliveCellsOfBitwiseAlgorithm();
        return CountAliveCellsOfSmartAlgorithm();
    }

    private int CountAliveCellsOfBitwiseAlgorithm()
    {
        var bitwiseAlgorithm = LifeLikeAutomationBitwiseAlgorithm.Instance;

        int columns = _automation.FieldSize.X,
            rows = _automation.FieldSize.Y;

        var aliveCellCount = 0;

        for (var y = 0; y < rows; y++)
            for (var x = 0; x < columns; x++)
                if (bitwiseAlgorithm.GetCellState(new(x, y)) is LifeLikeAutomationCellState.Alive)
                    aliveCellCount++;

        return aliveCellCount;
    }

    private static int CountAliveCellsOfSmartAlgorithm() => LifeLikeAutomationSmartAlgorithm.Instance.AliveCells.Count;
    #endregion
}