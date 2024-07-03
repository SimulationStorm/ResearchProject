using System.Collections.Generic;
using System;
using System.Linq;

public static class DrawingBrushShapeExtensions
{
    public static readonly IEnumerable<DrawingBrushShape> All = Enum.GetValues(typeof(DrawingBrushShape)).Cast<DrawingBrushShape>();

    private static readonly IReadOnlyDictionary<DrawingBrushShape, string> NamesByBrushShape = new Dictionary<DrawingBrushShape, string>
    {
        [DrawingBrushShape.Square] = "Квадрат",
        [DrawingBrushShape.Circle] = "Круг",
        [DrawingBrushShape.Triangle] = "Треугольник"
    };

    public static string Name(this DrawingBrushShape drawingBrushShape) => NamesByBrushShape[drawingBrushShape];

    public static DrawingBrushShape ByName(string name) => NamesByBrushShape.First(kv => kv.Value == name).Key;
}