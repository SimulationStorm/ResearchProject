using System;
using System.Collections.Generic;

public abstract class SimulationHistory<TRecord> : ISimulationHistory<TRecord> where TRecord : SimulationHistoryRecord
{
    public IReadOnlyList<TRecord> Records { get; }

    #region Events
    public event Action<TRecord>? RecordAdded,
                                  LastRecordOverwritten;
    
    public event Action? RecordsCleared;
    #endregion

    private readonly IList<TRecord> _records = new List<TRecord>();

    protected SimulationHistory() => Records = (IReadOnlyList<TRecord>)_records;

    protected abstract TRecord CreateRecord();

    #region Public methods
    public void MakeRecord()
    {
        var newRecord = CreateRecord();
        _records.Add(newRecord);
        RecordAdded?.Invoke(newRecord);
    }

    public void OverwriteLastRecord()
    {
        var newRecord = CreateRecord();

        if (_records.Count > 0)
        {
            _records[^1] = newRecord;
        }
        else
        {
            _records.Add(newRecord);
            RecordAdded?.Invoke(newRecord);
        }

        LastRecordOverwritten?.Invoke(newRecord);
    }

    public void Clear()
    {
        _records.Clear();
        RecordsCleared?.Invoke();
    }
    #endregion
}