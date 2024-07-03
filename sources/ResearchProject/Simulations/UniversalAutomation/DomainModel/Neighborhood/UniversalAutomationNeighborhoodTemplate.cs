using System;
using System.Linq;

public class UniversalAutomationNeighborhoodTemplate
{
    public string Name { get; }

    public Func<int, int, int, bool> PositionSelector { get; }

    public UniversalAutomationNeighborhoodTemplate(string name, Func<int, int, int, bool> positionSelector)
    {
        Name = name;
        PositionSelector = positionSelector;
    }

    public static readonly UniversalAutomationNeighborhoodTemplate
        Moore = new
        (
            "Окрестность Мура",
            (_, _, _) => true
        ),
        VonNeumann = new
        (
            "Окрестность фон Неймана",
            (radius, x, y) => Math.Abs(x) + Math.Abs(y) <= radius
        ),
        EuclidianDistance = new
        (
            "Евклидово расстояние",
            (radius, x, y) => Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5) <= radius
        ),
        Circle = new
        (
            "Круг",
            (radius, x, y) => Math.Pow(Math.Pow(x, 2) + Math.Pow(y, 2), 0.5) <= radius + 0.5
        ),
        Chessboard = new
        (
            "Шахматы",
            (_, x, y) => (x + y) % 2 is not 0
        ),
        InvertedChessboard = new
        (
            "Инвертированные шахматы",
            (_, x, y) => (x + y) % 2 is 0
        ),
        Grid = new
        (
            "Решётка",
            (_, x, y) => Math.Abs(x) is 1 || Math.Abs(y) is 1
        ),
        Cross = new
        (
            "Крест",
            (_, x, y) => x is 0 || y is 0
        ),
        Saltire = new
        (
            "Андреевский крест",
            (_, x, y) => Math.Abs(x) == Math.Abs(y)
        ),
        Star = new
        (
            "Звезда",
            (_, x, y) => x is 0 || y is 0 || Math.Abs(x) == Math.Abs(y)
        );

    public static readonly UniversalAutomationNeighborhoodTemplate[] All =
    {
        Moore,
        VonNeumann,
        EuclidianDistance,
        Circle,
        Chessboard,
        InvertedChessboard,
        Grid,
        Cross,
        Saltire,
        Star
    };

    public static UniversalAutomationNeighborhoodTemplate ByName(string name) => All.First(template => template.Name == name);
}