using EasyBindings;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp.Views.Godot;
using System.Collections.Generic;
using System.Linq;

public class LifeLikeAutomationPieChartVM : SimulationChartVM<LifeLikeAutomationHistoryRecord>
{
    public IEnumerable<ISeries> Series { get; }

    private readonly ObservableValue _deadCellCountValue = new(0),
                                     _aliveCellCountValue = new(0);

    public LifeLikeAutomationPieChartVM
    (
        IReadOnlySimulationHistory<LifeLikeAutomationHistoryRecord> history,
        LifeLikeAutomationPresentationModel presentationModel) : base(history)
    {
        if (history.Records.Count > 0)
            Update(history.Records.Last());

        var deadCellCountSeries = new PieSeries<ObservableValue>
        {
            Name = "Мёртвые клетки",
            Values = new[] { _deadCellCountValue },
            Fill = new SolidColorPaint(presentationModel.DeadCellColor.ToSKColor())
        };
        var aliveCellCountSeries = new PieSeries<ObservableValue>
        {
            Name = "Живые клетки",
            Values = new[] { _aliveCellCountValue },
            Fill = new SolidColorPaint(presentationModel.AliveCellColor.ToSKColor())
        };

        Series = new ISeries[]
        {
            deadCellCountSeries,
            aliveCellCountSeries
        };

        TriggerBinder.OnPropertyChanged(this, presentationModel, o => o.DeadCellColor, () =>
            deadCellCountSeries.Fill = new SolidColorPaint(presentationModel.DeadCellColor.ToSKColor()));

        TriggerBinder.OnPropertyChanged(this, presentationModel, o => o.AliveCellColor, () =>
            aliveCellCountSeries.Fill = new SolidColorPaint(presentationModel.AliveCellColor.ToSKColor()));
    }

    protected override void OnRecordAdded(LifeLikeAutomationHistoryRecord newRecord) => Update(newRecord);

    protected override void OnLastRecordOverwritten(LifeLikeAutomationHistoryRecord newRecord) => Update(newRecord);

    protected override void OnRecordsCleared()
    {
        _deadCellCountValue.Value = 0;
        _aliveCellCountValue.Value = 0;
    }

    private void Update(LifeLikeAutomationHistoryRecord record)
    {
        _deadCellCountValue.Value = record.DeadCellCount;
        _aliveCellCountValue.Value = record.AliveCellCount;
    }
}