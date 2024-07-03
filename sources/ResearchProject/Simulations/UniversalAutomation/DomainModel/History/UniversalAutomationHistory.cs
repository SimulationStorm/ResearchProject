using System.Collections.Generic;

public class UniversalAutomationHistory : SimulationHistory<UniversalAutomationHistoryRecord>
{
    private readonly UniversalAutomation _automation;

    public UniversalAutomationHistory(UniversalAutomation automation) => _automation = automation;

    protected override UniversalAutomationHistoryRecord CreateRecord()
    {
        var cellCountsByState = new Dictionary<byte, int>();
        foreach (var state in _automation.States)
            cellCountsByState[state] = 0;

        int columns = _automation.FieldSize.X,
            rows = _automation.FieldSize.Y;

        for (var x = 0; x < columns; x++)
            for (var y = 0; y < rows; y++)
                cellCountsByState[_automation.GetCellState(x, y)]++;

        return new UniversalAutomationHistoryRecord(cellCountsByState);
    }
}