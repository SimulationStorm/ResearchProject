using System.Collections.Generic;
using Godot;

public class UniversalAutomationRuleBuilder
{
    private double _probability = 1;

    private byte _oldState,
                 _newState;

    private UniversalAutomationNeighborhood? _neighborhood;

    private byte? _neighborState;

    private ISet<int>? _neighborCounts;
    private ISet<Vector2I>? _neighborPositions;

    public UniversalAutomationRuleBuilder HasProbability(double probability)
    {
        _probability = probability;
        return this;
    }

    public UniversalAutomationRuleBuilder HasOldState(UniversalAutomationState oldState) => HasOldState(oldState.Number);

    public UniversalAutomationRuleBuilder HasOldState(byte oldState)
    {
        _oldState = oldState;
        return this;
    }

    public UniversalAutomationRuleBuilder HasNewState(UniversalAutomationState newState) => HasNewState(newState.Number);

    public UniversalAutomationRuleBuilder HasNewState(byte newState)
    {
        _newState = newState;
        return this;
    }

    public UniversalAutomationRuleBuilder HasNeighborhood(UniversalAutomationNeighborhood neighborhood)
    {
        _neighborhood = neighborhood;
        return this;
    }

    public UniversalAutomationRuleBuilder HasNeighborState(UniversalAutomationState neighborState) => HasNeighborState(neighborState.Number);

    public UniversalAutomationRuleBuilder HasNeighborState(byte neighborState)
    {
        _neighborState = neighborState;
        return this;
    }

    public UniversalAutomationRuleBuilder HasNeighborCounts(params int[] neighborCounts)
    {
        _neighborCounts = new HashSet<int>(neighborCounts);
        return this;
    }

    public UniversalAutomationRuleBuilder HasNeighborCounts(ISet<int> neighborCounts)
    {
        _neighborCounts = neighborCounts;
        return this;
    }

    public UniversalAutomationRuleBuilder HasNeighborPositions(params Vector2I[] neighborPositions)
    {
        _neighborPositions = new HashSet<Vector2I>(neighborPositions);
        return this;
    }

    public UniversalAutomationRuleBuilder HasNeighborPositions(ISet<Vector2I> neighborPositions)
    {
        _neighborPositions = neighborPositions;
        return this;
    }

    public UniversalAutomationRule Build() => new
    (
        _oldState,
        _newState,
        _probability,
        _neighborhood,
        _neighborState,
        _neighborCounts,
        _neighborPositions
    );
}