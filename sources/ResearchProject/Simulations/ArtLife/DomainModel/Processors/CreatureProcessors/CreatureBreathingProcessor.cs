using System;
using System.Linq;

public class CreatureBreathingProcessor : AliveCreatureProcessor
{
    protected override void ProcessAliveCreature(Creature creature)
    {
        switch (creature.Properties.BreathState)
        {
            case CreatureBreathState.Inhale:
                var inhaleResult = ProcessInhale(creature);
                if (inhaleResult && creature.Properties.BreathSatisfaction < ArtLifeSettings.CreatureSatisfactionMaxValue)
                    creature.Properties.BreathSatisfaction++;
                else
                    creature.Properties.BreathSatisfaction--;

                break;
            case CreatureBreathState.Exhale:
                ProcessExhale(creature);
                break;
        }

        UpdateBreathState(creature);
    }

    private static bool ProcessInhale(Creature creature)
    {
        var inhaleSubstance = GetInhaleSubstance(creature);
        
        var cellsHavingInhaleSubstance = CellHelpers.GetSurroundingCellsHavingSubstance(creature.HabitatCell, inhaleSubstance);
        if (cellsHavingInhaleSubstance.Any() == false)
        {
            // Creature changes breath type and starts breathing on next iteration (if there is inhale substance)
            TryChangeBreathType(creature);
            return false;
        }

        var cellHavingInhaleSubstance = cellsHavingInhaleSubstance.RandomElementThreadSafe();

        cellHavingInhaleSubstance.Substances[inhaleSubstance] -= 1;
        creature.Properties.SubstancesInProcessing[inhaleSubstance] += 1;

        return true;
    }

    private static bool TryChangeBreathType(Creature creature)
    {
        if (creature.Genotype.BreathType != CreatureBreathType.Mixed)
            return false;

        CreatureBreathType currentBreathType = creature.Fenotype.BreathType,
                           newBreathType = GetOppositeBreathType(currentBreathType);

        creature.Fenotype.BreathType = newBreathType;

        return true;
    }

    private static CreatureBreathType GetOppositeBreathType(CreatureBreathType breathType) => breathType switch
    {
        CreatureBreathType.Aerobic => CreatureBreathType.Anaerobic,
        CreatureBreathType.Anaerobic => CreatureBreathType.Aerobic,

        _ => throw new NotImplementedException()
    };

    private static void ProcessExhale(Creature creature)
    {
        Substance inhaleSubstance = GetInhaleSubstance(creature),
                  exhaleSubstance = GetExhaleSubstance(creature);

        creature.Properties.SubstancesInProcessing[inhaleSubstance] -= 1;
        creature.HabitatCell.ExistingNeighborsAndSelf.RandomElementThreadSafe().Substances[exhaleSubstance] += 1;
    }

    private static Substance GetInhaleSubstance(Creature creature) => creature.Fenotype.BreathType switch
    {
        CreatureBreathType.Aerobic => Substance.Oxygen,
        CreatureBreathType.Anaerobic => Substance.CarbonDioxide,

        _ => throw new NotImplementedException()
    };

    private static Substance GetExhaleSubstance(Creature creature) => creature.Fenotype.BreathType switch
    {
        CreatureBreathType.Aerobic => Substance.CarbonDioxide,
        CreatureBreathType.Anaerobic => Substance.Oxygen,

        _ => throw new NotImplementedException()
    };

    private static void UpdateBreathState(Creature creature)
    {
        var inhaleSubstance = GetInhaleSubstance(creature);
        var breathSubstanceCapacityGene = creature.Genotype.BreathSubstanceCapacity;
        var inhaleSubstanceAmountInProcessing = creature.Properties.SubstancesInProcessing[inhaleSubstance];

        if (creature.Properties.BreathState == CreatureBreathState.Inhale && inhaleSubstanceAmountInProcessing == breathSubstanceCapacityGene)
            creature.Properties.BreathState = CreatureBreathState.Exhale;
        else if (creature.Properties.BreathState == CreatureBreathState.Exhale && inhaleSubstanceAmountInProcessing == 0)
            creature.Properties.BreathState = CreatureBreathState.Inhale;
    }
}