using System.Collections.Generic;

public class UniversalAutomationHistoryRecord : SimulationHistoryRecord
{
    public IReadOnlyDictionary<byte, int> CellCountsByStateNumber { get; }

    public UniversalAutomationHistoryRecord(IDictionary<byte, int> cellCountsByStateNumber) =>
        CellCountsByStateNumber = (IReadOnlyDictionary<byte, int>)cellCountsByStateNumber;
}