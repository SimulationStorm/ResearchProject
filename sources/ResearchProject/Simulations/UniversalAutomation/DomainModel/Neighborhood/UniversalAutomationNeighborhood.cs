using System;
using System.Collections.Generic;
using Godot;

public class UniversalAutomationNeighborhood
{
    #region Properties
    public int Radius { get; }

    public IReadOnlySet<Vector2I> SelectedPositions { get; }

    public UniversalAutomationNeighborhoodTemplate? Template { get; }
    #endregion

    public UniversalAutomationNeighborhood(int radius, ISet<Vector2I>? selectedPositions = null, UniversalAutomationNeighborhoodTemplate? template = null)
    {
        ValidateRadius(radius);
        Radius = radius;

        if (selectedPositions is null && template is null || selectedPositions is not null && template is not null)
            throw new Exception("Only one of selectedPositions or template should be given.");

        if (template is not null)
        {
            Template = template;
            selectedPositions = GetAllowedPositions(radius, template);
        }

        SelectedPositions = (IReadOnlySet<Vector2I>)selectedPositions!;
    }

    #region Public static methods
    public static int GetSide(int radius) => radius * 2 + 1;

    public static int GetMaxNeighborCount(int radius)
    {
        var side = GetSide(radius);
        return side * side - 1; // Don't include center
    }

    public static ISet<Vector2I> GetAllowedPositions(int radius)
    {
        var positions = new HashSet<Vector2I>();
        IterateOverAllowedPositions(radius, (position) => positions.Add(position));
        return positions;
    }

    public static ISet<Vector2I> GetAllowedPositions(int radius, UniversalAutomationNeighborhoodTemplate template)
    {
        var selectedPositions = new HashSet<Vector2I>();
        IterateOverAllowedPositions(radius, position =>
        {
            if (template.PositionSelector(radius, position.X, position.Y))
                selectedPositions.Add(position);
        });
        return selectedPositions;
    }

    public static void IterateOverNeighborCounts(int radius, Action<int> action)
    {
        var maxNeighborCount = GetMaxNeighborCount(radius);
        for (var neighborCount = 0; neighborCount <= maxNeighborCount; neighborCount++)
            action(neighborCount);
    }

    public static void IterateOverAllowedPositions(int radius, Action<Vector2I> action)
    {
        IterateOverAllPositions(radius, (position, isCenter) =>
        {
            if (!isCenter)
                action(position);
        });
    }

    public static void IterateOverAllPositions(int radius, Action<Vector2I, bool> action)
    {
        for (var x = -radius; x <= radius; x++)
            for (var y = -radius; y <= radius; y++)
                action(new(x, y), x is 0 && y is 0);
    }

    #region Validation methods
    public static void ValidateRadius(int radius)
    {
        if (radius < 1)
            throw new ArgumentException("Should be greater or equal to one.", nameof(radius));
    }

    public static void ValidateNeighborCounts(int radius, ISet<int> counts) 
    {
        ArgumentNullException.ThrowIfNull(counts, nameof(counts));

        foreach (var count in counts)
            ValidateNeighborCount(radius, count);
    }

    public static void ValidateNeighborCount(int radius, int count)
    {
        if (count < 0 || count > GetMaxNeighborCount(radius))
            throw new ArgumentException("Should be greater or equal to zero and less than position count in neighborhood.", nameof(count));
    }

    public static void ValidatePositions(int radius, ISet<Vector2I> positions)
    {
        ArgumentNullException.ThrowIfNull(positions, nameof(positions));

        foreach (var position in positions)
            ValidatePosition(radius, position);
    }

    public static void ValidatePosition(int radius, Vector2I position)
    {
        int x = position.X,
            y = position.Y;

        if (x < -radius || x > radius || y < -radius || y > radius)
            throw new ArgumentException("Position x and y should be in range [-Radius;Radius].", nameof(position));

        if (x is 0 && y is 0)
            throw new ArgumentException("It is not allowed to set position (0,0).", nameof(position));
    }
    #endregion
    #endregion
}