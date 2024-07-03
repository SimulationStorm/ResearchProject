using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using Godot;

public abstract partial class AutomationFieldUiVM<TCellState> : SimulationFieldUiVM
{
    #region Commands
    [RelayCommand(CanExecute = nameof(CanDraw))]
    private void DrawShape(Vector2I cell) => _automationModel.SetCellsState(GetShapePoints(cell), _presentationModel.DrawingBrushCellState);

    [RelayCommand(CanExecute = nameof(CanDraw))]
    private void DrawLineOfShapes((Vector2I from, Vector2I to) coordinates)
    {
        IEnumerable<Vector2I>
            linePoints = GetLinePoints(coordinates.from, coordinates.to),
            shapePoints = linePoints.SelectMany(GetShapePoints);

        _automationModel.SetCellsState(shapePoints, _presentationModel.DrawingBrushCellState);
    }

    private bool CanDraw() => _presentationModel.DrawingModeEnabled;
    #endregion

    #region Fields
    private readonly AutomationModel<TCellState> _automationModel;

    private readonly AutomationPresentationModel<TCellState> _presentationModel;
    #endregion

    protected AutomationFieldUiVM
    (
        FieldStateModel fieldStateModel,
        AutomationModel<TCellState> automationModel,
        AutomationPresentationModel<TCellState> presentationModel
    ) 
    : base(fieldStateModel)
    {
        _automationModel = automationModel;
        _presentationModel = presentationModel;
    }

    #region Methods
    private IEnumerable<Vector2I> GetShapePoints(Vector2I center)
    {
        var radius = _presentationModel.DrawingBrushRadius;
        if (radius is 0)
            return new[] { center };

        var shapeCellsGetter = GetShapePointsGetter(_presentationModel.DrawingBrushShape);
        var shapeCells = shapeCellsGetter(center, _presentationModel.DrawingBrushRadius);
        return GetVectorsWithinRect(shapeCells, new(0, 0, _automationModel.FieldSize));
    }

    #region Private static methods
    private static Func<Vector2I, int, IEnumerable<Vector2I>> GetShapePointsGetter(DrawingBrushShape drawingBrushShape) => drawingBrushShape switch
    {
        DrawingBrushShape.Square => GetSquarePoints,
        DrawingBrushShape.Circle => GetCirclePoints,
        DrawingBrushShape.Triangle => GetTrianglePoints
    };

    private static IEnumerable<Vector2I> GetLinePoints(Vector2I from, Vector2I to)
    {
        int xFrom = from.X,
            yFrom = from.Y,
            xTo = to.X,
            yTo = to.Y,
            deltaX = Math.Abs(xTo - xFrom),
            deltaY = Math.Abs(yTo - yFrom),
            signX = xFrom < xTo ? 1 : -1,
            signY = yFrom < yTo ? 1 : -1,
            error = deltaX - deltaY,
            currentX = xFrom,
            currentY = yFrom;

        while (currentX != xTo || currentY != yTo)
        {
            yield return new(currentX, currentY);

            var doubledError = error * 2;

            if (doubledError > -deltaY)
            {
                error -= deltaY;
                currentX += signX;
            }

            if (doubledError < deltaX)
            {
                error += deltaX;
                currentY += signY;
            }
        }

        yield return new(xTo, yTo); // The last cell at (xTo, yTo)
    }

    private static IEnumerable<Vector2I> GetSquarePoints(Vector2I center, int radius)
    {
        int diameter = radius * 2,
            startX = center.X - radius,
            startY = center.Y - radius,
            endX = startX + diameter,
            endY = startY + diameter;

        for (var x = startX; x <= endX; x++)
            for (var y = startY; y <= endY; y++)
                yield return new(x, y);
    }

    private static IEnumerable<Vector2I> GetCirclePoints(Vector2I center, int radius)
    {
        int x = radius,
            y = 0,
            radiusError = 1 - x;

        while (x >= y)
        {
            for (var i = -x; i <= x; i++)
            {
                yield return new Vector2I(center.X + i, center.Y + y);
                yield return new Vector2I(center.X + i, center.Y - y);
            }

            for (var i = -y; i <= y; i++)
            {
                yield return new Vector2I(center.X + i, center.Y + x);
                yield return new Vector2I(center.X + i, center.Y - x);
            }

            y++;
            if (radiusError < 0)
            {
                radiusError += 2 * y + 1;
            }
            else
            {
                x--;
                radiusError += 2 * (y - x + 1);
            }
        }
    }

    #region Triangle
    private static IEnumerable<Vector2I> GetTrianglePoints(Vector2I circleCenter, int circleRadius)
    {
        var triangleHeight = circleRadius * Math.Sqrt(3);

        Vector2I topVertex = new(circleCenter.X, circleCenter.Y - circleRadius),
                 leftVertex = new(circleCenter.X - circleRadius, circleCenter.Y + (int)(triangleHeight / 2)),
                 rightVertex = new(circleCenter.X + circleRadius, circleCenter.Y + (int)(triangleHeight / 2));

        int minX = Math.Min(topVertex.X, Math.Min(leftVertex.X, rightVertex.X)),
            maxX = Math.Max(topVertex.X, Math.Max(leftVertex.X, rightVertex.X)),
            minY = Math.Min(topVertex.Y, Math.Min(leftVertex.Y, rightVertex.Y)),
            maxY = Math.Max(topVertex.Y, Math.Max(leftVertex.Y, rightVertex.Y));

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var currentPoint = new Vector2I(x, y);
                if (IsPointInsideTriangle(currentPoint, topVertex, leftVertex, rightVertex))
                    yield return currentPoint;
            }
        }
    }

    private static bool IsPointInsideTriangle(Vector2I point, Vector2I v1, Vector2I v2, Vector2I v3)
    {
        int d1 = GetTriangleAreaSign(point, v1, v2),
            d2 = GetTriangleAreaSign(point, v2, v3),
            d3 = GetTriangleAreaSign(point, v3, v1);

        bool hasNegative = (d1 < 0) || (d2 < 0) || (d3 < 0),
             hasPositive = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNegative && hasPositive);
    }

    private static int GetTriangleAreaSign(Vector2I p1, Vector2I p2, Vector2I p3)
    {
        return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
    }
    #endregion

    private static IEnumerable<Vector2I> GetVectorsWithinRect(IEnumerable<Vector2I> vectors, Rect2I rect)
    {
        int minX = rect.Position.X,
            maxX = rect.End.X,
            minY = rect.Position.Y,
            maxY = rect.End.Y;

        return vectors.Where(v => v.X >= minX && v.X <= maxX && v.Y >= minY && v.Y <= maxY);
    }
    #endregion
    #endregion
}