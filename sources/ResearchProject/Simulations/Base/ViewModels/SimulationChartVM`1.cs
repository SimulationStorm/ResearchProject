using EasyBindings.Interfaces;

public abstract class SimulationChartVM<TRecord> : IUnsubscribe where TRecord : SimulationHistoryRecord
{
    protected readonly IReadOnlySimulationHistory<TRecord> History;

    protected SimulationChartVM(IReadOnlySimulationHistory<TRecord> history)
    {
        History = history;

        History.RecordAdded += OnRecordAdded;
        History.LastRecordOverwritten += OnLastRecordOverwritten;
        History.RecordsCleared += OnRecordsCleared;
    }

    #region Protected methods
    protected abstract void OnRecordAdded(TRecord newRecord);

    protected abstract void OnLastRecordOverwritten(TRecord newRecord);

    protected abstract void OnRecordsCleared();
    #endregion

    public virtual void Unsubscribe()
    {
        History.RecordAdded -= OnRecordAdded;
        History.LastRecordOverwritten -= OnLastRecordOverwritten;
        History.RecordsCleared -= OnRecordsCleared;
    }
}