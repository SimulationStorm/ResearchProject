using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class LifeLikeAutomationColumnChartVM : SimulationChartVM<LifeLikeAutomationHistoryRecord>
{
    public IEnumerable<ISeries> Series { get; }

    private readonly ObservableCollection<int> _values = new();

    public LifeLikeAutomationColumnChartVM(IReadOnlySimulationHistory<LifeLikeAutomationHistoryRecord> history) : base(history)
    {
        foreach (var record in history.Records)
            _values.Add(record.AliveCellCount);

        Series = new ISeries[]
        {
            new ColumnSeries<int>
            {
                Values = _values,
                Stroke = null,
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                IgnoresBarPosition = true
            }
        };
    }

    protected override void OnRecordAdded(LifeLikeAutomationHistoryRecord newRecord) =>
        _values.Add(newRecord.AliveCellCount);

    protected override void OnLastRecordOverwritten(LifeLikeAutomationHistoryRecord newRecord) =>
        _values[^1] = newRecord.AliveCellCount;

    protected override void OnRecordsCleared() => _values.Clear();
}