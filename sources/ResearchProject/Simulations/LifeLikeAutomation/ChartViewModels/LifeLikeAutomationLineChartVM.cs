using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

public partial class LifeLikeAutomationLineChartVM : SimulationChartVM<LifeLikeAutomationHistoryRecord>
{
    #region Properties
    public IEnumerable<ISeries> Series { get; }

    public IEnumerable<Axis> MainChartXAxes { get; }
    public IEnumerable<Axis> MainChartYAxes { get; }
    
    public IEnumerable<Axis> ScrollBarChartXAxes { get; } = new[] { new Axis { IsVisible = false } };
    public IEnumerable<Axis> ScrollbarChartYAxes { get; } = new[] { new Axis { IsVisible = false } };

    public IEnumerable<RectangularSection> Thumbs { get; }

    // force the left margin to be 100 and the right margin 50 in both charts, this will
    // align the start and end point of the "draw margin",
    // no matter the size of the labels in the Y axis of both chart.
    public readonly Margin Margin = new(100, Margin.Auto, 50, Margin.Auto);
    #endregion

    #region Commands
    [RelayCommand]
    private void ChartUpdated(ChartCommandArgs _)
    {
        // update the scroll bar thumb when the chart is updated (zoom/pan)
        // this will let the user know the current visible range
        _thumb.Xi = _mainChartXAxis.MinLimit;
        _thumb.Xj = _mainChartXAxis.MaxLimit;
    }


    [RelayCommand]
    private void PointerDown(PointerCommandArgs _) => _isPointerDown = true;


    [RelayCommand]
    private void PointerUp(PointerCommandArgs _) => _isPointerDown = false;


    [RelayCommand]
    private void PointerMove(PointerCommandArgs args)
    {
        if (!_isPointerDown)
            return;

        var chart = (ICartesianChartView<SkiaSharpDrawingContext>)args.Chart;
        var positionInData = chart.ScalePixelsToData(args.PointerPosition);

        var currentRange = _thumb.Xj - _thumb.Xi;
        // update the scroll bar thumb when the user is dragging the chart
        _thumb.Xi = positionInData.X - currentRange / 2;
        _thumb.Xj = positionInData.X + currentRange / 2;

        // update the chart visible range
        _mainChartXAxis.MinLimit = _thumb.Xi;
        _mainChartXAxis.MaxLimit = _thumb.Xj;
    }
    #endregion

    #region Fields
    private readonly Axis _mainChartXAxis = new()
    {
        Name = "№ итерации",
        NamePaint = new SolidColorPaint(SKColors.White),
        NameTextSize = 14,

        LabelsPaint = new SolidColorPaint(SKColors.White),
        TextSize = 14
    };

    private readonly Axis _mainChartYAxis = new()
    {
        Name = "Число живых клеток",
        NamePaint = new SolidColorPaint(SKColors.White),
        NameTextSize = 14,

        LabelsPaint = new SolidColorPaint(SKColors.White),
        TextSize = 14
    };

    private readonly RectangularSection _thumb = new()
    {
        Fill = new SolidColorPaint(new SKColor(255, 205, 210, 100))
    };

    private readonly ObservableCollection<int> _aliveCellCounts = new();

    private bool _isPointerDown;
    #endregion

    public LifeLikeAutomationLineChartVM(IReadOnlySimulationHistory<LifeLikeAutomationHistoryRecord> history) : base(history)
    {
        foreach (var record in history.Records)
            OnRecordAdded(record);

        Series = new ISeries[]
        {
            new LineSeries<int>
            {
                Values = _aliveCellCounts,
                //GeometryStroke = new SolidColorPaint(),
                //GeometryFill = new SolidColorPaint(SKColors.Blue),
                GeometrySize = 5,
                DataPadding = new(0, 1)
            }
        };

        MainChartXAxes = new[] { _mainChartXAxis };
        MainChartYAxes = new[] { _mainChartYAxis };

        Thumbs = new[] { _thumb };
    }

    protected override void OnRecordAdded(LifeLikeAutomationHistoryRecord newRecord) =>
        _aliveCellCounts.Add(newRecord.AliveCellCount);

    protected override void OnLastRecordOverwritten(LifeLikeAutomationHistoryRecord newRecord) =>
        _aliveCellCounts[^1] = newRecord.AliveCellCount;

    protected override void OnRecordsCleared() => _aliveCellCounts.Clear();
}