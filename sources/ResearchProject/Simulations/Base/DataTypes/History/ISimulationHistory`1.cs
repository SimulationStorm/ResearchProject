public interface ISimulationHistory<out TRecord> : IReadOnlySimulationHistory<TRecord> where TRecord : SimulationHistoryRecord
{
    void MakeRecord();

    void OverwriteLastRecord();

    void Clear();
}