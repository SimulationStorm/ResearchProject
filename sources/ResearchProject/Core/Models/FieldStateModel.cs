using CommunityToolkit.Mvvm.ComponentModel;
using Godot;
using System;
using System.Collections.Generic;

public partial class FieldStateModel : ObservableObject
{
    #region Properties
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FieldSize))]
    private Vector2I _screenSize;

    public Vector2I FieldSize => new((int)(ScreenSize.X / CellSize), (int)(ScreenSize.Y / CellSize));

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FieldSize))]
    private double _cellSize;

    public IReadOnlyList<double> AvailableCellSizes { get; }

    [ObservableProperty]
    private double _viewScale;

    [ObservableProperty]
    private bool _gridLinesShown;

    [ObservableProperty]
    private Color _gridLinesColor;
    #endregion

    public FieldStateModel()
    {
        ScreenSize = App.ScreenSize;
        ViewScale = FieldCameraSettings.InitialViewScale;
        GridLinesShown = FieldSettings.InitialGridLinesShown;
        GridLinesColor = FieldSettings.InitialGridLinesColor;

        AvailableCellSizes = GetAvailableSquareCellSizes(ScreenSize.X, ScreenSize.Y,
            FieldSettings.MinCellSize, FieldSettings.CellSizeStep);

        // Select average cell size
        CellSize = AvailableCellSizes[AvailableCellSizes.Count / 2];//[^1];
    }

    private static IReadOnlyList<double> GetAvailableSquareCellSizes(int fieldWidth, int fieldHeight, double minSize, double step)
    {
        var cellSizes = new List<double>();

        var maxSide = Math.Max(fieldWidth, fieldHeight);
        for (var i = minSize; i < maxSide; i += step)
            if (fieldWidth % i == 0 && fieldHeight % i == 0)
                cellSizes.Add(i);

        return cellSizes;
    }
}