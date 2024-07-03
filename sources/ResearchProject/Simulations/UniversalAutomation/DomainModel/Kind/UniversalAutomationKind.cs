using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

public class UniversalAutomationKind
{
    #region Properties
    public string Name { get; }

    public int RuleSetsRepetitionsNumber { get; }

    public ISet<UniversalAutomationState> States { get; }

    public UniversalAutomationState DefaultState { get; }

    public IEnumerable<UniversalAutomationRuleSet> RuleSets { get; }
    #endregion

    #region Setting up
    public UniversalAutomationKind
    (
        string name,
        int ruleSetsRepetitionsNumber,
        ISet<UniversalAutomationState> states,
        UniversalAutomationState defaultState,
        IEnumerable<UniversalAutomationRuleSet> ruleSets)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));

        RuleSetsRepetitionsNumber = ruleSetsRepetitionsNumber > 0
            ? ruleSetsRepetitionsNumber
            : throw new ArgumentOutOfRangeException(nameof(ruleSetsRepetitionsNumber), "Should be greater than zero.");

        ArgumentNullException.ThrowIfNull(states, nameof(states));
        if (states.IsEmpty())
            throw new ArgumentException("Should include at least one state.", nameof(states));

        States = states;

        if (!states.Contains(defaultState))
            throw new ArgumentException("State should be in list of states to mark it default.", nameof(states));

        DefaultState = defaultState;

        ArgumentNullException.ThrowIfNull(ruleSets, nameof(ruleSets));
        if (ruleSets.IsEmpty())
            throw new ArgumentException("Should include at least one state.", nameof(ruleSets));

        RuleSets = ruleSets;
    }
    #endregion

    public static readonly UniversalAutomationKind
        GameOfLife = BuildGameOfLife(),
        WireWorld = BuildWireWorld();

    private static readonly UniversalAutomationKind[] All =
    {
        // Life family
        GameOfLife,

        WireWorld,

        // By Stephen Wolfram
    };

    public static UniversalAutomationKind ByName(string name) => All.First(k => k.Name == name);

    #region Kind builders
    private static UniversalAutomationKind BuildGameOfLife()
    {
        UniversalAutomationState
            deadState = new UniversalAutomationStateBuilder()
                .HasName("Мертва")
                .HasNumber(1)
                .HasColor(Colors.White)
                .Build(),
            aliveState = new UniversalAutomationStateBuilder()
                .HasName("Жива")
                .HasNumber(2)
                .HasColor(Colors.Black)
                .Build();

        return new UniversalAutomationKindBuilder()
            .HasName("Игра \"Жизнь\"")
            .HasRuleSetsRepetitionsNumber(1)
            .HasDefaultState(deadState)
            .HasState(aliveState)
            .HasRuleSet(new UniversalAutomationRuleSetBuilder()
                .HasApplicationsNumber(1)
                .HasRule(new UniversalAutomationRuleBuilder()
                    .HasOldState(deadState)
                    .HasNewState(aliveState)
                    .HasProbability(0.5)
                    .Build())
                .Build())
            .HasRuleSet(new UniversalAutomationRuleSetBuilder()
                .HasApplicationsNumber(1)
                .HasRule(new UniversalAutomationRuleBuilder()
                    .HasOldState(deadState)
                    .HasNewState(aliveState)
                    .HasNeighborhood(new UniversalAutomationNeighborhoodBuilder()
                        .HasRadius(1)
                        .HasTemplate(UniversalAutomationNeighborhoodTemplate.Moore)
                        .Build())
                    .HasNeighborState(aliveState)
                    .HasNeighborCounts(3)
                    .Build())
                .HasRule(new UniversalAutomationRuleBuilder()
                    .HasOldState(aliveState)
                    .HasNewState(deadState)
                    .HasNeighborhood(new UniversalAutomationNeighborhoodBuilder()
                        .HasRadius(1)
                        .HasTemplate(UniversalAutomationNeighborhoodTemplate.Moore)
                        .Build())
                    .HasNeighborState(deadState)
                    .HasNeighborCounts(0, 1, 2, 3, 4, 7, 8)
                    .Build())
                .Build())
            .Build();
    }

    private static UniversalAutomationKind BuildWireWorld()
    {
        UniversalAutomationState
            emptyState = new UniversalAutomationStateBuilder()
                .HasName("Пустая")
                .HasNumber(1)
                .HasColor(Color.FromHtml("#111111"))
                .Build(),
            signalState = new UniversalAutomationStateBuilder()
                .HasName("Сигнал")
                .HasNumber(2)
                .HasColor(Color.FromHtml("#EF4949"))
                .Build(),
            signalTailState = new UniversalAutomationStateBuilder()
                .HasName("Хвост сигнала")
                .HasNumber(3)
                .HasColor(Color.FromHtml("49D0EF"))
                .Build(),
            wireState = new UniversalAutomationStateBuilder()
                .HasName("Проводник")
                .HasNumber(4)
                .HasColor(Color.FromHtml("#EFB949"))
                .Build();

        return new UniversalAutomationKindBuilder()
            .HasName("Мир проводов")
            .HasRuleSetsRepetitionsNumber(1)
            .HasDefaultState(emptyState)
            .HasState(signalState)
            .HasState(signalTailState)
            .HasState(wireState)
            .HasRuleSet(new UniversalAutomationRuleSetBuilder()
                .HasApplicationsNumber(1)
                .HasRule(new UniversalAutomationRuleBuilder()
                    .HasOldState(signalState)
                    .HasNewState(signalTailState)
                    .Build())
                .HasRule(new UniversalAutomationRuleBuilder()
                    .HasOldState(signalTailState)
                    .HasNewState(wireState)
                    .Build())
                .HasRule(new UniversalAutomationRuleBuilder()
                    .HasOldState(wireState)
                    .HasNewState(signalState)
                    .HasNeighborhood(new UniversalAutomationNeighborhoodBuilder()
                        .HasRadius(1)
                        .HasTemplate(UniversalAutomationNeighborhoodTemplate.Moore)
                        .Build())
                    .HasNeighborState(signalState)
                    .HasNeighborCounts(1, 2)
                    .Build())
                .Build())
            .Build();
    }
    #endregion
}