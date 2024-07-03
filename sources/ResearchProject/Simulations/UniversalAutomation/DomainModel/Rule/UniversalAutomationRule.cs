using System;
using System.Collections.Generic;
using Godot;

public class UniversalAutomationRule
{
    #region Properties
    #region Unconditional part
    /// <summary>
    /// Probability of applying the rule
    /// </summary>
    public double Probability { get; }

    /// <summary>
    /// The old state of the cell
    /// </summary>
    public byte OldState { get; }

    /// <summary>
    /// The new state of the cell
    /// </summary>
    public byte NewState { get; }
    
    /// <summary>
    /// The rule type
    /// </summary>
    public UniversalAutomationRuleType Type { get; }
    #endregion

    #region Conditional part
    /// <summary>
    /// The neighborhood
    /// </summary>
    public UniversalAutomationNeighborhood? Neighborhood { get; }

    /// <summary>
    /// The state of the cell neighbor or neighbors
    /// </summary>
    public byte? NeighborState { get; }

    /// <summary>
    /// Used when <see cref="Type"/> is Totalistic
    /// 
    /// Allowed counts of neighbor cells in <see cref="Neighborhood"/>
    /// with <see cref="NeighborState"/> under which the rule will be applied
    /// 
    /// Min value is 0
    /// Max value is the number of neighbors in <see cref="Neighborhood"/>
    /// </summary>
    public IReadOnlySet<int>? NeighborCounts { get; }

    /// <summary>
    /// Used when <see cref="Type"/> is Nontotalistic
    /// 
    /// Allowed positions of neighbor cells in <see cref="Neighborhood"/>
    /// with <see cref="NeighborState"/> under which the rule will be applied
    /// </summary>
    public IReadOnlySet<Vector2I>? NeighborPositions { get; }
    #endregion
    #endregion

    public UniversalAutomationRule
    (
        byte oldState,
        byte newState,
        double? probability = 1,
        UniversalAutomationNeighborhood? neighborhood = null,
        byte? neighborState = null,
        ISet<int>? neighborCounts = null,
        ISet<Vector2I>? neighborPositions = null)
    {
        OldState = oldState;
        NewState = newState;

        Probability = probability switch
        {
            >= 0 and <= 1 => probability.Value,
            _ => throw new ArgumentException("Should be in range [0;1].", nameof(probability))
        };

        bool isNeighborhoodGiven = neighborhood is not null,
             isNeighborCountsGiven = neighborCounts is not null,
             isNeighborPositionsGiven = neighborPositions is not null,
             isNeighborCountsOrPositionsGiven = isNeighborCountsGiven || isNeighborPositionsGiven;

        if (isNeighborCountsGiven && isNeighborPositionsGiven)
            throw new ArgumentException("Only one of neighbor counts/positions should be given.");

        if (isNeighborhoodGiven && !isNeighborCountsOrPositionsGiven)
            throw new ArgumentException("Along with neighborhood neighbor counts/positions should be given.");

        if (isNeighborCountsOrPositionsGiven && !isNeighborhoodGiven)
            throw new ArgumentException("Along with neighbor counts/positions neighborhood should be given.");

        if (neighborState is null && (isNeighborhoodGiven || isNeighborCountsOrPositionsGiven))
            throw new ArgumentException("Along with neighbor counts/positions and neighborhood neighbor state should be given.");

        if (neighborState is not null && (!isNeighborhoodGiven || !isNeighborCountsOrPositionsGiven))
            throw new ArgumentException("Along with neighbor state counts/positions and neighborhood should be given.");

        if (isNeighborhoodGiven)
        {
            Neighborhood = neighborhood;

            if (isNeighborCountsGiven)
            {
                UniversalAutomationNeighborhood.ValidateNeighborCounts(Neighborhood!.Radius, neighborCounts!);
                NeighborCounts = neighborCounts as IReadOnlySet<int>;
            }
            else if (isNeighborPositionsGiven)
            {
                UniversalAutomationNeighborhood.ValidatePositions(Neighborhood!.Radius, neighborPositions!);
                NeighborPositions = neighborPositions as IReadOnlySet<Vector2I>;
            }
        }

        NeighborState = neighborState;

        Type = isNeighborCountsGiven
            ? UniversalAutomationRuleType.Totalistic
            : isNeighborPositionsGiven
                ? UniversalAutomationRuleType.Nontotalistic
                : UniversalAutomationRuleType.Unconditional;
    }
}