using System;
using System.Collections.Generic;

public interface IReadOnlySimulationHistory<out TRecord> where TRecord : SimulationHistoryRecord
{
    public IReadOnlyList<TRecord> Records { get; }

    #region Events
    public event Action<TRecord>? RecordAdded,
                                  LastRecordOverwritten;

    public event Action? RecordsCleared;
    #endregion
}