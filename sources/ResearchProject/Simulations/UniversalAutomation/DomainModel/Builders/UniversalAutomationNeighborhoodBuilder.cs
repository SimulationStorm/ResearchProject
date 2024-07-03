using System.Collections.Generic;
using Godot;

public class UniversalAutomationNeighborhoodBuilder
{
    private int _radius;

    private ISet<Vector2I>? _selectedPositions;

    private UniversalAutomationNeighborhoodTemplate? _template;

    public UniversalAutomationNeighborhoodBuilder HasRadius(int radius)
    {
        _radius = radius;
        return this;
    }

    public UniversalAutomationNeighborhoodBuilder HasSelectedPositions(ISet<Vector2I> selectedPositions)
    {
        _selectedPositions = selectedPositions;
        return this;
    }

    public UniversalAutomationNeighborhoodBuilder HasTemplate(UniversalAutomationNeighborhoodTemplate template)
    {
        _template = template;
        return this;
    }

    public UniversalAutomationNeighborhood Build() => new
    (
        _radius,
        _selectedPositions,
        _template
    );
}