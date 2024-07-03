using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp.Views.Godot;

public class UniversalAutomationPieChartVM : SimulationChartVM<UniversalAutomationHistoryRecord>
{
    public ReadOnlyObservableCollection<ISeries> Series { get; }

    private readonly IDictionary<byte, ObservableValue> _observableValuesByState = new Dictionary<byte, ObservableValue>();

    private readonly IDictionary<byte, PieSeries<ObservableValue>> _seriesByState = new Dictionary<byte, PieSeries<ObservableValue>>();

    private readonly ObservableCollection<ISeries> _series = new();

    private readonly UniversalAutomationPresentationModel _presentationModel;

    public UniversalAutomationPieChartVM
    (
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel
    )
    : base(automationModel.History)
    {
        _presentationModel = presentationModel;

        Series = new ReadOnlyObservableCollection<ISeries>(_series);

        foreach (var state in automationModel.States)
            CreateObservableValueAndSeriesForState(state);

        if (History.Records.Count > 0)
            UpdateObservableValuesByRecord(History.Records[^1]);

        _presentationModel.StateAdded += OnStateAdded;
        _presentationModel.StateNameChanged += OnStateNameChanged;
        _presentationModel.StateColorChanged += OnStateColorChanged;
        _presentationModel.StateRemoved += OnStateRemoved;
    }

    #region Protected methods
    protected override void OnRecordAdded(UniversalAutomationHistoryRecord newRecord) => UpdateObservableValuesByRecord(newRecord);

    protected override void OnLastRecordOverwritten(UniversalAutomationHistoryRecord newRecord) => UpdateObservableValuesByRecord(newRecord);

    protected override void OnRecordsCleared()
    {
        foreach (var (_, observableValue) in _observableValuesByState)
            observableValue.Value = 0;
    }
    #endregion

    #region Private methods
    private void UpdateObservableValuesByRecord(UniversalAutomationHistoryRecord record)
    {
        foreach (var (state, cellCount) in record.CellCountsByStateNumber)
            _observableValuesByState[state].Value = cellCount;
    }

    private void OnStateAdded(byte state) => CreateObservableValueAndSeriesForState(state);

    private void OnStateNameChanged(byte state)
    {
        var name = _presentationModel.NamesByState[state];
        _seriesByState[state].Name = name;
    }

    private void OnStateColorChanged(byte state)
    {
        var color = _presentationModel.ColorsByState[state];
        _seriesByState[state].Fill = new SolidColorPaint(color.ToSKColor());
    }

    private void OnStateRemoved(byte state)
    {
        _observableValuesByState.Remove(state);

        var series = _seriesByState[state];
        _series.Remove(series);
        _seriesByState.Remove(state);
    }

    private void CreateObservableValueAndSeriesForState(byte state)
    {
        var name = _presentationModel.NamesByState[state];
        var color = _presentationModel.ColorsByState[state];

        var observableValue = new ObservableValue(0);
        var series = new PieSeries<ObservableValue>
        {
            Name = name,
            Values = new[] { observableValue },
            Fill = new SolidColorPaint(color.ToSKColor())
        };
        _observableValuesByState[state] = observableValue;
        _seriesByState[state] = series;
        _series.Add(series);
    }
    #endregion

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        _presentationModel.StateAdded -= OnStateAdded;
        _presentationModel.StateNameChanged -= OnStateNameChanged;
        _presentationModel.StateColorChanged -= OnStateColorChanged;
        _presentationModel.StateRemoved -= OnStateRemoved;
    }
}