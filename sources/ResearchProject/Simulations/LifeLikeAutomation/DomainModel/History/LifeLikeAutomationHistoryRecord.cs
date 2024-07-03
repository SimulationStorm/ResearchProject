public class LifeLikeAutomationHistoryRecord : SimulationHistoryRecord
{
    public int DeadCellCount { get; init; }

    public int AliveCellCount { get; init; }

    //public IReadOnlyDictionary<LifeLikeAutomationCellState, int> CellCountsByState { get; }

    //public LifeLikeAutomationHistoryRecord(IDictionary<LifeLikeAutomationCellState, int> cellCountsByState)
    //{
    //    CellCountsByState = (IReadOnlyDictionary<LifeLikeAutomationCellState, int>)cellCountsByState;
    //}
}